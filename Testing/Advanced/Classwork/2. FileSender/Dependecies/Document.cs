using System;

namespace Advanced.Classwork.FileSender.Dependecies;

public record Document(string Name, byte[] Content, DateTime Created, string Format);