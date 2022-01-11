using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SignalRApi.Entities.User;
using SignalRApi.Exceptions;
using SignalRApi.Repositories;
using SignalRApi.ViewModels;

namespace SignalRApi.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly CurrentUserService _currentUserService;

        public UserService(IUserRepository userRepository, JwtService jwtService, IMapper mapper, CurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<string> Login(string username, string useremail, string userpassword, string clientId, CancellationToken cancelationToken = default)
        {
            var userEntity = await _userRepository.Login(username, useremail, userpassword, cancelationToken);

            if (userEntity is null)
            {
                throw new NotFoundException(nameof(UserEntity));
            }

            return _jwtService
                .CreateJwtToken(userEntity.Id, userEntity.Name, userEntity.Email, userEntity.Roles?.Select(role => role.Name).ToArray() ?? new string[] { }, clientId);
        }

        public async Task<string> Register(string username, string useremail, string userpassword, string clientId, CancellationToken cancellationToken = default)
        {
            var newUserEntity = await _userRepository.Register(username, useremail, userpassword, cancellationToken);

            return _jwtService
                .CreateJwtToken(newUserEntity.Id, newUserEntity.Name, newUserEntity.Email, newUserEntity.Roles?.Select(role => role.Name).ToArray() ?? new string[] { }, clientId);
        }

        public async Task<UserViewModel> GetUserByEmail(string email, bool isIncludeRoles, CancellationToken cancellationToken = default)
        {
            var userEntity = await _userRepository.GetUserByEmail(email, isIncludeRoles, cancellationToken);
            return _mapper.Map<UserViewModel>(userEntity);
        }

        public async Task<List<UserViewModel>> GetUsersByName(string name, CancellationToken cancellationToken = default)
        {
            var list = await _userRepository.GetUsersByName(name, cancellationToken);
            return _mapper.Map<List<UserViewModel>>(list).Where(user => user.Id != _currentUserService.CurrentUserId()).ToList();
        }

        public async Task UpdateUserFirebaseTokens(string newToken, string oldToken, CancellationToken cancellationToken = default)
        {
            await _userRepository.UpdateUserFirebaseToken(_currentUserService.CurrentUserId(), newToken, oldToken, cancellationToken);
        }
    }
}
