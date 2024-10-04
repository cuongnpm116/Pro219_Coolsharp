using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAppIntegrated.SignalR;

public class SignalRService : IAsyncDisposable
{
    public HubConnection? _hubConnection;
    private readonly IConfiguration _configuration;
   

    public SignalRService(IConfiguration configuration )
    {
        _configuration = configuration;
       
    }

    public async Task InitializeSignalRConnection(List<Claim> claims)
    {
        string? secretKey = _configuration["Jwt:SecretKey"];
        var tokenHandler = new JwtSecurityTokenHandler();
        var identity = new ClaimsIdentity(claims, "token");
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256Signature)
        });

        var tokenString = tokenHandler.WriteToken(token);

        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:1000/shophub", options =>
            {
                options.AccessTokenProvider = async () => await Task.FromResult(tokenString);
            }).Build();

        await _hubConnection.StartAsync();

        if (_hubConnection.State != HubConnectionState.Connected)
        {
            throw new InvalidOperationException("Failed to connect to SignalR hub.");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
