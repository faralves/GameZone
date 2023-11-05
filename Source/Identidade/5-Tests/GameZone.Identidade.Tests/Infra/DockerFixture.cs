using Docker.DotNet;
using Docker.DotNet.Models;

namespace GameZone.Identidade.Tests.Api.Infra
{
    public class DockerFixture : IDisposable
    {
        private bool _disposed = false;
        private DockerClient _dockerClient;
        private string _containerId;
        private string containerName = "sql-server-Tests";

        public DockerFixture()
        {
            _dockerClient = new DockerClientConfiguration().CreateClient();
            _dockerClient.DefaultTimeout = new TimeSpan(0,10,0);

            var createContainerResponse = _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Name = containerName,
                Image = "mcr.microsoft.com/mssql/server:2019-latest",
                Env = new List<string>
                    {
                        "ACCEPT_EULA=Y",
                        "SA_PASSWORD=Mudar123intrA"
                    },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>()
                        {
                            { "1433/tcp", new List<PortBinding> { new PortBinding { HostPort = "1433" } } }
                        },
                    PublishAllPorts = true // Optional: Set this to true if you want to publish all exposed ports
                }
            }).GetAwaiter().GetResult();

            _containerId = createContainerResponse.ID;

            _dockerClient.Containers.StartContainerAsync(_containerId, new ContainerStartParameters()).GetAwaiter().GetResult();

        }

        public string GetConnectionString()
        {
            var _connectionString = $"Server=localhost,1433;Database=GameZoneDB;User Id=SA;Password=Mudar123intrA;MultipleActiveResultSets=true;TrustServerCertificate=true;";
            return _connectionString;
        }

        public async void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Parar e remover o contêiner
                    if (!string.IsNullOrEmpty(_containerId))
                    {
                        _dockerClient.Containers.StopContainerAsync(_containerId, new ContainerStopParameters { WaitBeforeKillSeconds = 10 });
                        _dockerClient.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters { Force = true });
                    }
                }

                // Libere outros recursos gerenciados, se houver

                _disposed = true;
            }
        }

        ~DockerFixture()
        {
            Dispose(false);
        }
    }
}