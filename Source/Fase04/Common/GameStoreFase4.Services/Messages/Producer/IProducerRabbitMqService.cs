﻿namespace GameStoreFase4.Services.Messages.Producer;
public interface IProducerRabbitMqService
{
    Task Publish(string message, Type objectType, bool publishDlq = false);
}