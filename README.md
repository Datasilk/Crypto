![Datasilk Logo](http://www.markentingh.com/projects/datasilk/logo.png)

# Crypto
#### State-of-the-art cryptography for C# .NET Core using ChaCha20

Encrypt or decrypt your data with the same method!

## Examples

```
class MyClass
{
    private UInt512 chachaKey = new UInt512(
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U,
        0X00000000U
    );

    public void EncryptFile(string text, string filePath)
    {
        byte[] data = new byte[text.Length * sizeof(char)];
        Buffer.BlockCopy(text.ToCharArray(), 0, data, 0, data.Length);

        var chacha = new ChaCha20(chachaKey);
        chacha.Transform(data);

        File.WriteAllBytes(filePath), data);
    }

    public string DecryptFile(string filePath)
    {
        var data = File.ReadAllBytes(filePath);
        var chacha = new ChaCha20(chachaKey);
        chacha.Transform(data);
        return Encoding.UTF8.GetString(data);
    }
}
```

