using Fighters.Application;
using Fighters.Models.Fighters;
using Moq;

namespace Fighters.Tests.Application
{
    public class GameMasterTests : IDisposable
    {
        private readonly TextReader _originalIn;
        private readonly TextWriter _originalOut;
        private readonly Mock<IFighter> _fighterMock1;
        private readonly Mock<IFighter> _fighterMock2;
        private readonly Mock<IFighter> _fighterMock3;

        public GameMasterTests()
        {
            _originalIn = Console.In;
            _originalOut = Console.Out;
            Console.SetOut( new StringWriter() );

            _fighterMock1 = CreateFighterMock( "Fighter1", 10, 100, 100 );
            _fighterMock2 = CreateFighterMock( "Fighter2", 8, 10, 15 );
            _fighterMock3 = CreateFighterMock( "Fighter3", 6, 10, 10 );
        }

        public void Dispose()
        {
            Console.SetIn( _originalIn );
            Console.SetOut( _originalOut );
        }

        private Mock<IFighter> CreateFighterMock( string name, int skill, int initialHealth, int damage )
        {
            Mock<IFighter> mock = new();
            mock.Setup( f => f.Name ).Returns( name );
            mock.Setup( f => f.Skill ).Returns( skill );

            int currentHealth = initialHealth;
            mock.Setup( f => f.CurrentHealth ).Returns( () => currentHealth );

            mock.Setup( f => f.TakeDamage( It.IsAny<int>() ) )
                .Callback<int>( dmg =>
                {
                    currentHealth = Math.Max( 0, currentHealth - Math.Max( 0, dmg ) );
                    mock.Setup( f => f.CurrentHealth ).Returns( () => currentHealth );
                } );

            mock.Setup( f => f.CalculateDamage() ).Returns( damage );
            return mock;
        }

        [Fact]
        public void PlayAndGetWinner_WithCorrectFighters_ShouldReturnCorrectWinner()
        {
            List<IFighter> fighters = [ _fighterMock1.Object, _fighterMock2.Object ];
            GameMaster gameMaster = new();
            StringWriter output = new();
            Console.SetOut( output );

            IFighter winner = gameMaster.PlayAndGetWinner( fighters );

            Assert.Same( _fighterMock1.Object, winner );
            string outputText = output.ToString();
            Assert.Contains( "Раунд 1", outputText );
            Assert.Contains( "Fighter1 атакует бойца Fighter2", outputText );
            Assert.Contains( "Fighter2 убит", outputText );
        }

        [Fact]
        public void PlayAndGetWinner_WithMoreThanMinimumFighters_ShouldHandle()
        {
            List<IFighter> fighters = [ _fighterMock1.Object, _fighterMock2.Object, _fighterMock3.Object ];
            GameMaster gameMaster = new();
            StringWriter output = new();
            Console.SetOut( output );

            IFighter winner = gameMaster.PlayAndGetWinner( fighters );

            Assert.Same( _fighterMock1.Object, winner );
            string outputText = output.ToString();
            Assert.Contains( "Раунд 1", outputText );
            Assert.Contains( "Раунд 2", outputText, StringComparison.OrdinalIgnoreCase );
            Assert.Contains( "убит", outputText );
        }

        [Fact]
        public void PlayAndGetWinner_ShouldOrderFightersBySkillDescending()
        {
            List<IFighter> fighters = [ _fighterMock2.Object, _fighterMock3.Object, _fighterMock1.Object ];
            GameMaster gameMaster = new();
            StringWriter output = new();
            Console.SetOut( output );

            IFighter winner = gameMaster.PlayAndGetWinner( fighters );

            Assert.Same( _fighterMock1.Object, winner );
            string outputText = output.ToString();
            Assert.Contains( "Fighter1 атакует", outputText );
        }

        [Fact]
        public void PlayAndGetWinner_WithCorrectPlay_ShouldRemoveDeadFighters()
        {
            IFighter weakFighter = CreateFighterMock( "WeakFighter", 5, 10, 5 ).Object;
            List<IFighter> fighters = [ _fighterMock1.Object, weakFighter ];
            GameMaster gameMaster = new();
            StringWriter output = new();
            Console.SetOut( output );

            IFighter winner = gameMaster.PlayAndGetWinner( fighters );

            Assert.Same( _fighterMock1.Object, winner );
            string outputText = output.ToString();
            Assert.Contains( "WeakFighter убит", outputText );
            Assert.DoesNotContain( "WeakFighter атакует", outputText );
        }

        [Fact]
        public void Fight_WithTwoFighters_ShouldApplyDamageCorrectly()
        {
            Mock<IFighter> attackerMock = CreateFighterMock( "Attacker", 10, 100, 20 );
            Mock<IFighter> defenderMock = CreateFighterMock( "Defender", 8, 50, 15 );

            GameMaster gameMaster = new();
            IFighter attacker = attackerMock.Object;
            IFighter defender = defenderMock.Object;

            int initialHealth = defender.CurrentHealth;
            int damage = attacker.CalculateDamage();

            StringWriter output = new();
            Console.SetOut( output );

            System.Reflection.MethodInfo? fightMethod = typeof( GameMaster ).GetMethod( "Fight",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance );
            fightMethod.Invoke( gameMaster, [ attacker, defender ] );

            Assert.Equal( initialHealth - damage, defender.CurrentHealth );
            string outputText = output.ToString();
            Assert.Contains( $"получает {damage} урона", outputText );
        }

        [Fact]
        public void CheckDeath_WithDeadFighter_ShouldReturnTrue()
        {
            GameMaster gameMaster = new();
            Mock<IFighter> deadFighter = new();
            deadFighter.Setup( f => f.CurrentHealth ).Returns( 0 );

            System.Reflection.MethodInfo? checkDeathMethod = typeof( GameMaster ).GetMethod( "CheckDeath",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance );
            bool isDead = ( bool )checkDeathMethod.Invoke( gameMaster, [ deadFighter.Object ] );

            Assert.True( isDead );
        }

        [Fact]
        public void CheckDeath_WithAliveFighter_ShouldReturnFalse()
        {
            GameMaster gameMaster = new();
            Mock<IFighter> aliveFighter = new();
            aliveFighter.Setup( f => f.CurrentHealth ).Returns( 10 );

            System.Reflection.MethodInfo? checkDeathMethod = typeof( GameMaster ).GetMethod( "CheckDeath",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance );
            bool isDead = ( bool )checkDeathMethod.Invoke( gameMaster, [ aliveFighter.Object ] );

            Assert.False( isDead );
        }

        [Fact]
        public void PlayAndGetWinner_MultipleRounds_ShouldHandle()
        {
            IFighter tankFighter = CreateFighterMock( "Tank", 7, 50, 10 ).Object;
            IFighter attackerFighter = CreateFighterMock( "Attacker", 9, 100, 30 ).Object;
            List<IFighter> fighters = [ attackerFighter, tankFighter ];
            GameMaster gameMaster = new();
            StringWriter output = new();
            Console.SetOut( output );

            IFighter winner = gameMaster.PlayAndGetWinner( fighters );

            Assert.NotNull( winner );
            string outputText = output.ToString();
            Assert.Matches( @"Раунд \d+", outputText );
            Assert.Contains( "атакует", outputText );
        }

        [Fact]
        public void PlayAndGetWinner_WithMinimumFighters_ShouldNotAllowSelfAttack()
        {
            List<IFighter> fighters = [ _fighterMock1.Object, _fighterMock2.Object ];
            GameMaster gameMaster = new();
            StringWriter output = new();
            Console.SetOut( output );

            IFighter winner = gameMaster.PlayAndGetWinner( fighters );

            string outputText = output.ToString();
            Assert.DoesNotContain( "Fighter1 атакует бойца Fighter1", outputText );
        }

        [Fact]
        public void PlayAndGetWinner_ShouldHandleImmediateDeath()
        {
            IFighter weakFighter = CreateFighterMock( "WeakFighter", 5, 1, 5 ).Object;
            IFighter strongFighter = CreateFighterMock( "StrongFighter", 10, 100, 100 ).Object;
            List<IFighter> fighters = [ strongFighter, weakFighter ];
            GameMaster gameMaster = new();
            StringWriter output = new();
            Console.SetOut( output );

            IFighter winner = gameMaster.PlayAndGetWinner( fighters );

            Assert.Same( strongFighter, winner );
            string outputText = output.ToString();
            Assert.Contains( "WeakFighter убит", outputText );
            Assert.DoesNotContain( "WeakFighter атакует", outputText );
        }
    }
}