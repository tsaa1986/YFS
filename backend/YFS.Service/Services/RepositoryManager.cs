using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;

        private IUserAuthenticationRepository _userAuthenticationRepository;
        private IAccountGroupRepository _accountGroupRepository;
        private IAccountTypeRepository _accountTypeRepository;
        private IAccountRepository _accountRepository;
        private ICurrencyRepository _currencyRepository;
        private ICategoryRepository _categoryRepository;

        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        private IMapper _mapper;
        private IConfiguration _configuration;

        public RepositoryManager(RepositoryContext repositoryContext, 
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper, 
            IConfiguration configuration)
        {
            _repositoryContext = repositoryContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public IUserAuthenticationRepository UserAuthentication
        {
            get
            {
                if (_userAuthenticationRepository is null)
                    _userAuthenticationRepository = new UserAuthenticationRepository(_userManager, _roleManager, 
                        _configuration, _mapper);
                return _userAuthenticationRepository;
            }
        }
        public IAccountGroupRepository AccountGroup
        {
            get
            {
                if (_accountGroupRepository is null)
                    _accountGroupRepository = new AccountGroupRepository(_repositoryContext);
                return _accountGroupRepository;
            }
        }
        public IAccountTypeRepository AccountType
        {
            get
            {
                if (_accountTypeRepository is null)
                    _accountTypeRepository = new AccountTypeRepository(_repositoryContext);
                return _accountTypeRepository;
            }
        }
        public IAccountRepository Account
        {
            get
            {
                if (_accountRepository is null)
                    _accountRepository = new AccountRepository(_repositoryContext);
                return _accountRepository;
            }
        }
        public ICurrencyRepository Currency
        {
            get
            {
                if (_currencyRepository is null)
                    _currencyRepository = new CurrencyRepository(_repositoryContext);
                return _currencyRepository;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_categoryRepository is null)
                    _categoryRepository = new CategoryRepository(_repositoryContext);
                return _categoryRepository;
            }
        }

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}

