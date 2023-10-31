using Bogus;
using GameZone.Identidade.Application.DTOs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Identidade.Tests.Api.Fixtures
{
    public class LoginUsuarioTestsFixture
    {
        private readonly Faker<LoginUsuarioDto> _faker;

        public LoginUsuarioTestsFixture()
        {
            _faker = new Faker<LoginUsuarioDto>()
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.Password, f => f.Internet.Password());
        }

        public LoginUsuarioDto LoginUserSuccess()
        {
            var loginUser = _faker.Generate();

            return loginUser;
        }
        public LoginUsuarioDto LoginUserWithEmailEmpty()
        {
            _faker.RuleFor(u => u.Email, string.Empty);
            var loginUser = _faker.Generate();

            return loginUser;
        }

        public LoginUsuarioDto LoginUserWithEmailMaxLength()
        {
            _faker.RuleFor(u => u.Email, f => f.Random.String2(257, "asfdasga@asfdgbasgba.com"));
            var loginUser = _faker.Generate();

            return loginUser;
        }

        public LoginUsuarioDto LoginUserWithEmailIsNotValid()
        {
            _faker.RuleFor(u => u.Email, f => f.Random.String2(256));
            var loginUser = _faker.Generate();

            return loginUser;
        }


        public LoginUsuarioDto LoginUserWithPasswordIsEmpty()
        {
            _faker.RuleFor(u => u.Password, string.Empty);
            var loginUser = _faker.Generate();

            return loginUser;
        }

        public LoginUsuarioDto LoginUserWithPasswordMaxLength()
        {
            _faker.RuleFor(u => u.Password, f => f.Random.String2(21));
            var loginUser = _faker.Generate();

            return loginUser;
        }
    }
}
