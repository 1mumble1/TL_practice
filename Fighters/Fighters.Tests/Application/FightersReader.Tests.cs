using Fighters.Application;
using Fighters.Application.Factory;
using Fighters.Models.Fighters;
using Moq;

namespace Fighters.Tests.Application
{
    public class FightersReaderTests : IDisposable
    {
        private readonly TextReader _originalIn;
        private readonly TextWriter _originalOut;
        private readonly Mock<IFactory> _factoryMock;
        private readonly Mock<IFighter> _fighterMock1;
        private readonly Mock<IFighter> _fighterMock2;

        public FightersReaderTests()
        {
            _originalIn = Console.In;
            _originalOut = Console.Out;
            _factoryMock = new Mock<IFactory>();

            _fighterMock1 = new Mock<IFighter>();
            _fighterMock2 = new Mock<IFighter>();

            // Настраиваем моки бойцов
            _factoryMock.SetupSequence( f => f.CreateFighter() )
                .Returns( _fighterMock1.Object )
                .Returns( _fighterMock2.Object );
        }

        public void Dispose()
        {
            Console.SetIn( _originalIn );
            Console.SetOut( _originalOut );
        }

        [Fact]
        public void ReadFighters_Correctly_ShouldAddFightersUntilEndCommandWithSufficientCount()
        {
            string inputCommands = "add-fighter\nadd-fighter\nend\n";
            StringReader input = new( inputCommands );
            Console.SetIn( input );
            Console.SetOut( new StringWriter() );

            FightersReader reader = new( _factoryMock.Object );

            // Act
            IReadOnlyList<IFighter> result = reader.ReadFighters();

            // Assert
            Assert.Equal( 2, result.Count );
            Assert.Same( _fighterMock1.Object, result[ 0 ] );
            Assert.Same( _fighterMock2.Object, result[ 1 ] );
            _factoryMock.Verify( f => f.CreateFighter(), Times.Exactly( 2 ) );
        }

        [Fact]
        public void ReadFighters_OnlyOneFighter_ShouldRejectEndCommandWithLessThanTwoFighters()
        {
            string inputCommands = "add-fighter\nend\nadd-fighter\nend\n";
            StringReader input = new( inputCommands );
            StringWriter output = new();
            Console.SetIn( input );
            Console.SetOut( output );

            FightersReader reader = new( _factoryMock.Object );

            IReadOnlyList<IFighter> result = reader.ReadFighters();

            string outputText = output.ToString();
            Assert.Contains( "Вы создали недостаточно бойцов для игры!", outputText );
            Assert.Equal( 2, result.Count );
        }

        [Theory]
        [InlineData( "add\nfighter\nadd-fighter\nadd-fighter\nend\n" )]
        [InlineData( "invalid\nadd-fighter\nadd-fighter\nend\n" )]
        public void ReadFighters_UnknowCommand_ShouldHandle( string inputCommands )
        {
            StringReader input = new( inputCommands );
            StringWriter output = new();
            Console.SetIn( input );
            Console.SetOut( output );

            FightersReader reader = new( _factoryMock.Object );

            IReadOnlyList<IFighter> result = reader.ReadFighters();

            string outputText = output.ToString();
            Assert.Contains( "Неизвестная команда, попробуйте еще раз", outputText );
            Assert.Equal( 2, result.Count );
        }

        [Fact]
        public void ReadFighters_WithMoreThanMinFighters_ShouldHandle()
        {
            Mock<IFighter> fighterMock3 = new Mock<IFighter>();
            _factoryMock.SetupSequence( f => f.CreateFighter() )
                .Returns( _fighterMock1.Object )
                .Returns( _fighterMock2.Object )
                .Returns( fighterMock3.Object );

            string inputCommands = "add-fighter\nadd-fighter\nadd-fighter\nend\n";
            StringReader input = new( inputCommands );
            Console.SetIn( input );
            Console.SetOut( new StringWriter() );

            FightersReader reader = new( _factoryMock.Object );

            IReadOnlyList<IFighter> result = reader.ReadFighters();

            Assert.Equal( 3, result.Count );
        }

        [Fact]
        public void ReadFighters_Correctly_ShouldReturnImmutableList()
        {
            string inputCommands = "add-fighter\nadd-fighter\nend\n";
            StringReader input = new StringReader( inputCommands );
            Console.SetIn( input );
            Console.SetOut( new StringWriter() );

            FightersReader reader = new( _factoryMock.Object );

            IReadOnlyList<IFighter> result = reader.ReadFighters();

            Assert.IsAssignableFrom<IReadOnlyList<IFighter>>( result );
        }

        [Fact]
        public void ReadFighters_ShouldDisplayCorrectCommandPrompts()
        {
            string inputCommands = "add-fighter\nadd-fighter\nend\n";
            StringReader input = new( inputCommands );
            StringWriter output = new();
            Console.SetIn( input );
            Console.SetOut( output );

            FightersReader reader = new( _factoryMock.Object );

            _ = reader.ReadFighters();

            string outputText = output.ToString();
            Assert.Contains( "Введите команду:", outputText );
            Assert.Contains( "add-fighter - Добавить нового бойца на арену", outputText );
            Assert.Contains( "end - Завершить добавление бойцов", outputText );
        }
    }
}