using CSharp_PG2.Managers.Object;
using NUnit.Framework;

[TestFixture]
public class FaceTest
{
    [Test]
    public void FromString_OnlyVertexIndices_ParsesCorrectly()
    {
        // Arrange
        string faceString = "1 2 3";

        // Act
        var face = Face.FromString(faceString);

        // Assert
        Assert.AreEqual(3, face.VertexIndices.Count);
        Assert.AreEqual(1, face.VertexIndices[0]);
        Assert.AreEqual(2, face.VertexIndices[1]);
        Assert.AreEqual(3, face.VertexIndices[2]);
        Assert.IsEmpty(face.TextureIndices);
        Assert.IsEmpty(face.NormalIndices);
    }

    [Test]
    public void FromString_VertexAndTextureIndices_ParsesCorrectly()
    {
        // Arrange
        string faceString = "1/1 2/2 3/3";

        // Act
        var face = Face.FromString(faceString);

        // Assert
        Assert.AreEqual(3, face.VertexIndices.Count);
        Assert.AreEqual(1, face.VertexIndices[0]);
        Assert.AreEqual(2, face.VertexIndices[1]);
        Assert.AreEqual(3, face.VertexIndices[2]);
        Assert.AreEqual(1, face.TextureIndices[0]);
        Assert.AreEqual(2, face.TextureIndices[1]);
        Assert.AreEqual(3, face.TextureIndices[2]);
        Assert.IsEmpty(face.NormalIndices);
    }

    [Test]
    public void FromString_VertexTextureAndNormalIndices_ParsesCorrectly()
    {
        // Arrange
        string faceString = "1/1/1 2/2/2 3/3/3";

        // Act
        var face = Face.FromString(faceString);

        // Assert
        Assert.AreEqual(3, face.VertexIndices.Count);
        Assert.AreEqual(1, face.VertexIndices[0]);
        Assert.AreEqual(2, face.VertexIndices[1]);
        Assert.AreEqual(3, face.VertexIndices[2]);
        Assert.AreEqual(1, face.TextureIndices[0]);
        Assert.AreEqual(2, face.TextureIndices[1]);
        Assert.AreEqual(3, face.TextureIndices[2]);
        Assert.AreEqual(1, face.NormalIndices[0]);
        Assert.AreEqual(2, face.NormalIndices[1]);
        Assert.AreEqual(3, face.NormalIndices[2]);
    }

    [Test]
    public void FromString_InvalidIndices_ReturnsEmptyFace()
    {
        // Arrange
        string faceString = "1/abc/1 2/2/ 3//3";

        // Act
        var face = Face.FromString(faceString);

        // Assert
        Assert.IsEmpty(face.VertexIndices);
        Assert.IsEmpty(face.TextureIndices);
        Assert.IsEmpty(face.NormalIndices);
    }
}
