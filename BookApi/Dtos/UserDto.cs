//Data Transfer Object  for receiving User when registering or logging in
public class UserDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
