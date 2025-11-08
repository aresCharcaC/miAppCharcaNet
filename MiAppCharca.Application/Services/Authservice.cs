using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;
using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            // Buscar usuario
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

            // Verificar contraseña
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValidPassword)
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

            // Obtener roles
            var roles = await _roleRepository.GetRolesByUserIdAsync(user.UserId);
            var roleNames = roles.Select(r => r.RoleName).ToList();

            // Generar token
            var token = GenerateJwtToken(user.UserId, user.Username, roleNames);
            
            var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60");

            return new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email ?? string.Empty,
                Roles = roleNames,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Verificar si el usuario ya existe
            if (await _userRepository.UsernameExistsAsync(request.Username))
                throw new InvalidOperationException("El nombre de usuario ya está en uso");

            // Encriptar contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Crear usuario
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            // Asignar rol por defecto
            var role = await _roleRepository.GetByNameAsync(request.RoleName);
            if (role != null)
            {
                var userRole = new UserRole
                {
                    UserId = user.UserId,
                    RoleId = role.RoleId,
                    AssignedAt = DateTime.UtcNow
                };
                // Nota: Necesitarás agregar el UserRole al contexto aquí
            }

            return new RegisterResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email ?? string.Empty,
                Message = "Usuario registrado exitosamente"
            };
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public string GenerateJwtToken(Guid userId, string username, List<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "60");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Agregar roles como claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}