using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Identidade.Tests.Api.Infra
{
    public class DatabaseFixture : IClassFixture<DockerFixture>
    {
        private readonly DockerFixture _dockerFixture;

        public DatabaseFixture(DockerFixture dockerFixture)
        {
            _dockerFixture = dockerFixture;
        }
    }
}
