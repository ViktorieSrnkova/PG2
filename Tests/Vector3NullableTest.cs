using CSharp_PG2.Containers;

namespace Tests;

[TestFixture]
public class Vector3NullableTest
{

    [Test]
    public void AddTest()
    {
        var a = new Vector3Nullable(1, null, 2);
        var b = new Vector3Nullable(3, 4, null);
        
        var c = a + b;
        Assert.Multiple(() =>
        {
            Assert.That(c.X, Is.EqualTo(4));
            Assert.That(c.Y, Is.EqualTo(4));
        });
    }

    [Test]
    public void ArrayAccessTest()
    {
        var a = new Vector3Nullable(1, null, 2);
        Assert.That(a[0], Is.EqualTo(1));
        Assert.That(a[1], Is.Null);
        Assert.That(a[2], Is.EqualTo(2));
        
        Assert.That(() => a[3], Throws.Exception);
    }

    [Test]
    public void ArraySetTest()
    {
        var a = new Vector3Nullable();

        a[0] = 1;
        Assert.That(a[0], Is.EqualTo(1));
    }
}