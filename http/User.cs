namespace http;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }

    public override string ToString() => $"Id:{Id} Name:{Name} Surname:{Surname} Age:{Age}";

}