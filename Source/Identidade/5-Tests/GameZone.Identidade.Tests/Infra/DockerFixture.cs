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
            _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            InitializeAsync().Wait();
        }

        public bool VerificarContainerAtivo()
        {
            var container = GetContainerAsync(containerName).GetAwaiter().GetResult();

            if (container != null)
            {
                if (!container.State.Running)
                {
                    StartContainerAsync(container.ID).GetAwaiter().GetResult();
                }
                return true;
            }
            return false;
        }

        public async Task<ContainerInspectResponse?> GetContainerInitializeIfExists()
        {
            var container = await GetContainerAsync(containerName);

            if (container != null)
            {
                if (!container.State.Running)
                {
                    StartContainerAsync(container.ID).GetAwaiter().GetResult();
                }
            }

            return container;
        }

        private async Task StartContainerAsync(string containerId)
        {
            await _dockerClient.Containers.StartContainerAsync(containerId, new ContainerStartParameters());
        }


        private async Task<ContainerInspectResponse> GetContainerAsync(string containerName)
        {
            try
            {
                return await _dockerClient.Containers.InspectContainerAsync(containerName);
            }
            catch (Docker.DotNet.DockerContainerNotFoundException)
            {
                return null;
            }
        }


        private async Task InitializeAsync()
        {
            var sqlServerImage = "mcr.microsoft.com/mssql/server:2019-latest";

            //ContainerInspectResponse? container = await GetContainerInitializeIfExists();

            //if (container == null)
            //{
                //var existingImages = await _dockerClient.Images.ListImagesAsync(new ImagesListParameters());

                //if (existingImages.Any(image => image.RepoTags.Contains(sqlServerImage)))
                //{
                    var createContainerResponse = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
                    {
                        Name = containerName,
                        Image = sqlServerImage,
                        Env = new List<string>
                    {
                        "ACCEPT_EULA=Y",
                        "SA_PASSWORD=Mudar123intrA"
                    },
                        HostConfig = new HostConfig
                        {
                            PortBindings = new Dictionary<string, IList<PortBinding>>()
                        {
                            { "1433/tcp", new List<PortBinding> { new PortBinding { HostPort = "1436" } } }
                        },
                            PublishAllPorts = true // Optional: Set this to true if you want to publish all exposed ports
                        }
                    });

                    _containerId = createContainerResponse.ID;

                    await _dockerClient.Containers.StartContainerAsync(_containerId, new ContainerStartParameters());
                //}
                //else
                //    throw new Exception("É necessário baixar a imgem do SQL - 'docker pull mcr.microsoft.com/mssql/server:2019-latest'");

            //}
        }

        public string GetConnectionString()
        {
            var _connectionString = $"Server=localhost,1436;Database=GameZoneDB;User Id=SA;Password=Mudar123intrA;MultipleActiveResultSets=true;TrustServerCertificate=true;";
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