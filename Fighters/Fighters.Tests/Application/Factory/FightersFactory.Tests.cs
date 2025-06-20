using Fighters.Application.Factory;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Tests.Application.Factory;

public class FightersFactoryTests : IDisposable
{
    private readonly TextReader _originalIn;
    private readonly TextWriter _originalOut;
    private readonly FightersFactory _factory = new();

    public FightersFactoryTests()
    {
        _originalIn = Console.In;
        _originalOut = Console.Out;
    }

    public void Dispose()
    {
        Console.SetIn( _originalIn );
        Console.SetOut( _originalOut );
    }


    [Fact]
    public void CreateFighter_WithCorrectParams_ShouldCreateCorrectFighter()
    {
        StringReader input = new( "TestFighter\n1\n1\n1\n1" );
        Console.SetIn( input );
        StringWriter output = new();
        Console.SetOut( output );

        IFighter fighter = _factory.CreateFighter();

        Assert.Equal( "TestFighter", fighter.Name );
        Assert.IsType<Human>( fighter.Race );
        Assert.IsType<Warrior>( fighter.Class );
        Assert.IsType<Sword>( fighter.Weapon );
        Assert.IsType<Breastplate>( fighter.Armor );

        string outputText = output.ToString();
        Assert.Contains( "Боец 'TestFighter' был успешно добавлен!", outputText );
        Assert.Contains( "здоровье:", outputText );
        Assert.Contains( "броня:", outputText );
        Assert.Contains( "урон:", outputText );
        Assert.Contains( "скилл:", outputText );
    }

    [Theory]
    [InlineData( "1", typeof( Human ) )]
    [InlineData( "2", typeof( Elf ) )]
    [InlineData( "3", typeof( Witcher ) )]
    public void ChooseRace_WithCorrectOption_ShouldReturnCorrectType( string inputChoice, Type expectedType )
    {
        Console.SetIn( new StringReader( inputChoice ) );
        StringWriter output = new();
        Console.SetOut( output );

        IRace? race = _factory.GetType()
            .GetMethod( "ChooseRace", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static )
            .Invoke( null, null ) as IRace;

        Assert.IsType( expectedType, race );
    }

    [Theory]
    [InlineData( "1", typeof( Warrior ) )]
    [InlineData( "2", typeof( Knight ) )]
    [InlineData( "3", typeof( Assassin ) )]
    public void ChooseClass_ShouldReturnCorrectType( string inputChoice, Type expectedType )
    {
        // Arrange
        Console.SetIn( new StringReader( inputChoice ) );
        StringWriter output = new();
        Console.SetOut( output );

        // Act
        IClass? fighterClass = _factory.GetType()
            .GetMethod( "ChooseClass", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static )
            .Invoke( null, null ) as IClass;

        // Assert
        Assert.IsType( expectedType, fighterClass );
    }

    [Theory]
    [InlineData( "1", typeof( Sword ) )]
    [InlineData( "2", typeof( Knife ) )]
    [InlineData( "3", typeof( Hammer ) )]
    [InlineData( "4", typeof( NoWeapon ) )]
    public void ChooseWeapon_ShouldReturnCorrectType( string inputChoice, Type expectedType )
    {
        // Arrange
        Console.SetIn( new StringReader( inputChoice ) );
        StringWriter output = new();
        Console.SetOut( output );

        // Act
        IWeapon? weapon = _factory.GetType()
            .GetMethod( "ChooseWeapon", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static )
            .Invoke( null, null ) as IWeapon;

        // Assert
        Assert.IsType( expectedType, weapon );
    }

    [Theory]
    [InlineData( "1", typeof( Breastplate ) )]
    [InlineData( "2", typeof( Greaves ) )]
    [InlineData( "3", typeof( Helmet ) )]
    [InlineData( "4", typeof( NoArmor ) )]
    public void ChooseArmor_ShouldReturnCorrectType( string inputChoice, Type expectedType )
    {
        // Arrange
        Console.SetIn( new StringReader( inputChoice ) );
        StringWriter output = new();
        Console.SetOut( output );

        // Act
        IArmor? armor = _factory.GetType()
            .GetMethod( "ChooseArmor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static )
            .Invoke( null, null ) as IArmor;

        // Assert
        Assert.IsType( expectedType, armor );
    }

    [Fact]
    public void CreateFighter_WithEmptyName_ShouldHandleInvalidName()
    {
        // Arrange
        StringReader input = new( "\n \nValidName\n1\n1\n1\n1" );
        Console.SetIn( input );
        StringWriter output = new();
        Console.SetOut( output );

        // Act
        var fighter = _factory.CreateFighter();

        // Assert
        Assert.Equal( "ValidName", fighter.Name );
        var outputText = output.ToString();
        Assert.Contains( "Невалидное имя для бойца, попробуйте еще раз", outputText );
    }

    [Theory]
    [InlineData( "0\n4\n1", typeof( Human ) )]
    [InlineData( "abc\n2", typeof( Elf ) )]
    [InlineData( "99\n-1\n3", typeof( Witcher ) )]
    public void ChooseRace_WithInvalidOptions_ShouldHandleInvalidInput( string inputSequence, Type expectedType )
    {
        // Arrange
        Console.SetIn( new StringReader( inputSequence ) );
        StringWriter output = new();
        Console.SetOut( output );

        // Act
        IRace? race = _factory.GetType()
            .GetMethod( "ChooseRace", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static )
            .Invoke( null, null ) as IRace;

        // Assert
        Assert.IsType( expectedType, race );
        string outputText = output.ToString();
        Assert.Contains( "Необходимо ввести число от 1 до 3!", outputText );
    }

    [Theory]
    [InlineData( "0\n1", 1, 3, 1 )]
    [InlineData( "4\n3", 1, 3, 3 )]
    [InlineData( "abc\n2", 1, 3, 2 )]
    public void GetOption_WithInvalidOption_ShouldValidateInputRange( string input, int min, int max, int expected )
    {
        System.Reflection.MethodInfo? method = _factory.GetType().GetMethod( "GetOption",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static );

        Console.SetIn( new StringReader( input ) );
        int result = ( int )method.Invoke( null, [ min, max ] );
        Assert.Equal( expected, result );
    }

    [Fact]
    public void CreateFighter_WithCorrectParams_ShouldCalculateStatsCorrectly()
    {
        StringReader input = new( "StatsTest\n1\n1\n1\n1" );
        Console.SetIn( input );
        StringWriter output = new();
        Console.SetOut( output );

        var fighter = _factory.CreateFighter();

        int expectedHealth = new Human().Health + new Warrior().Health;
        int expectedArmor = new Human().Armor + new Breastplate().Armor;
        int expectedDamage = new Human().Damage + new Warrior().Damage + new Sword().Damage;
        int expectedSkill = new Human().Skill + new Warrior().Skill;

        Assert.Equal( expectedHealth, fighter.MaxHealth );
        Assert.Equal( expectedArmor, fighter.MaxArmor );
        Assert.Equal( expectedDamage, fighter.Damage );
        Assert.Equal( expectedSkill, fighter.Skill );
    }
}