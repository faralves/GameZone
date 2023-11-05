using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using GameZone.Identidade.Infra;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace GameZone.Identidade.Tests.Api.Infra
{
    public class DockerFixture : IDisposable
    {
        private bool _disposed = false;
        private DockerClient _dockerClient;
        private string _containerId;

        public DockerFixture()
        {
            InitializeAsync().Wait();
        }

        //public bool VerificarContainerAtivo()
        //{
        //    bool ativo = false;
        //    _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

        //    var containerName = "sql-server-Tests";
        //    var sqlServerImage = "mcr.microsoft.com/mssql/server:2019-latest";

        //    var containers = _dockerClient.Containers.ListContainersAsync(new ContainersListParameters { All = true }).GetAwaiter().GetResult();
        //    bool containerExists = containers.Any(container => container.Names.Contains("/" + containerName));

        //    if (containerExists)
        //    {
        //        var existingContainer = containers.FirstOrDefault(container => container.Names.Contains("/" + containerName));

        //        if (existingContainer.State != "running")
        //        {
        //            // O contêiner existe, mas não está em execução; você pode iniciar o contêiner.
        //            _dockerClient.Containers.StartContainerAsync(existingContainer.ID, new ContainerStartParameters()).GetAwaiter().GetResult();
        //        }
        //        else
        //            ativo = true;
        //    }
        //    return ativo;
        //}

        public bool VerificarContainerAtivo()
        {
            _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            var containerName = "sql-server-Tests";

            var containers = _dockerClient.Containers.ListContainersAsync(new ContainersListParameters { All = true }).GetAwaiter();

            var existingContainer = containers.GetResult().FirstOrDefault(container => container.Names.Contains("/" + containerName));

            if (existingContainer != null)
            {
                if (existingContainer.State != "running")
                {
                    _dockerClient.Containers.StartContainerAsync(existingContainer.ID, new ContainerStartParameters());
                }
                else
                {
                    
                    return true;
                }
            }

            return false; 
        }


        private async Task InitializeAsync()
        {
            _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

            var containerName = "sql-server-Tests";
            var sqlServerImage = "mcr.microsoft.com/mssql/server:2019-latest";

            var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters { All = true });
            bool containerExists = containers.Any(container => container.Names.Contains("/" + containerName));

            if (containerExists)
            {
                var existingContainer = containers.FirstOrDefault(container => container.Names.Contains("/" + containerName));

                if (existingContainer.State != "running")
                {
                    // O contêiner existe, mas não está em execução; você pode iniciar o contêiner.
                   await _dockerClient.Containers.StartContainerAsync(existingContainer.ID, new ContainerStartParameters());
                }
            }
            else
            {
                var existingImages = await _dockerClient.Images.ListImagesAsync(new ImagesListParameters());

                if (existingImages.Any(image => image.RepoTags.Contains(sqlServerImage)))
                {
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
                            { "1433/tcp", new List<PortBinding> { new PortBinding { HostPort = "1433" } } }
                        },
                            PublishAllPorts = true // Optional: Set this to true if you want to publish all exposed ports
                        }
                    });

                    _containerId = createContainerResponse.ID;

                    await _dockerClient.Containers.StartContainerAsync(_containerId, new ContainerStartParameters());
                }
                else
                    throw new Exception("É necessário baixar a imgem do SQL - 'docker pull mcr.microsoft.com/mssql/server:2019-latest'");

            }
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
