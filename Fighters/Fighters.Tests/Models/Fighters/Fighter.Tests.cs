using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Moq;

namespace Fighters.Tests.Models.Fighters;

public class FighterTests
{
    private readonly Mock<IRace> _raceMock;
    private readonly Mock<IClass> _classMock;
    private readonly Mock<IWeapon> _weaponMock;
    private readonly Mock<IArmor> _armorMock;
    private readonly Fighter _fighter;

    public FighterTests()
    {
        _raceMock = new Mock<IRace>();
        _raceMock.Setup( r => r.Health ).Returns( 100 );
        _raceMock.Setup( r => r.Damage ).Returns( 10 );
        _raceMock.Setup( r => r.Skill ).Returns( 5 );
        _raceMock.Setup( r => r.Armor ).Returns( 3 );

        _classMock = new Mock<IClass>();
        _classMock.Setup( c => c.Health ).Returns( 50 );
        _classMock.Setup( c => c.Damage ).Returns( 5 );
        _classMock.Setup( c => c.Skill ).Returns( 2 );

        _weaponMock = new Mock<IWeapon>();
        _weaponMock.Setup( w => w.Damage ).Returns( 15 );

        _armorMock = new Mock<IArmor>();
        _armorMock.Setup( a => a.Armor ).Returns( 7 );

        _fighter = new Fighter(
            "TestFighter",
            _raceMock.Object,
            _classMock.Object,
            _weaponMock.Object,
            _armorMock.Object
        );
    }

    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        Assert.Equal( "TestFighter", _fighter.Name );
        Assert.Equal( _raceMock.Object, _fighter.Race );
        Assert.Equal( _classMock.Object, _fighter.Class );
        Assert.Equal( _weaponMock.Object, _fighter.Weapon );
        Assert.Equal( _armorMock.Object, _fighter.Armor );

        Assert.Equal( 150, _fighter.MaxHealth );
        Assert.Equal( 150, _fighter.CurrentHealth );
        Assert.Equal( 7, _fighter.Skill );
        Assert.Equal( 10, _fighter.MaxArmor );
        Assert.Equal( 10, _fighter.CurrentArmor );
        Assert.Equal( 30, _fighter.Damage );
    }

    [Theory]
    [InlineData( 0, 150, 10 )]
    [InlineData( 5, 150, 9 )]
    [InlineData( 10, 150, 8 )]
    [InlineData( 15, 145, 7 )]
    [InlineData( 160, 0, 0 )]
    public void TakeDamage_WithCorrectDamage_ShouldCorrectlyReduceArmorAndHealth( int damage, int expectedHealth, int expectedArmor )
    {
        _fighter.TakeDamage( damage );
        Assert.Equal( expectedArmor, _fighter.CurrentArmor );
        Assert.Equal( expectedHealth, _fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_WithNonPositiveDamage_ShouldNotReduceArmorAndHealth()
    {
        _fighter.TakeDamage( -10 );
        Assert.Equal( 10, _fighter.CurrentArmor );
        Assert.Equal( 150, _fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_WithLargeDamage_DoesNotReduceHealthBelowZero()
    {
        _fighter.TakeDamage( 1000 );
        Assert.Equal( 0, _fighter.CurrentArmor );
        Assert.Equal( 0, _fighter.CurrentHealth );
    }

    [Fact]
    public void CalculateDamage_ReturnsValueWithinExpectedRange()
    {
        int damage = _fighter.CalculateDamage();
        Assert.InRange( damage, 24, 66 );
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        string result = _fighter.ToString();
        Assert.Equal( "У бойца TestFighter: здоровье - 150, броня - 10", result );
    }
}