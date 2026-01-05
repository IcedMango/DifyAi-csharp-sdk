// Global using directives for integration tests
global using Xunit;
global using Xunit.Sdk;
global using FluentAssertions;

global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Net;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Logging.Console;

global using DifyAi.Configuration;
global using DifyAi.Dto.Base;
global using DifyAi.Dto.ParamDto;
global using DifyAi.Dto.ResDto;
global using DifyAi.Interface;
global using DifyAi.InternalException;
global using DifyAi.ServiceExtension;
global using DifyAi.Services;

global using IcedMango.DifyAi.IntegrationTests.Fixtures;
global using IcedMango.DifyAi.IntegrationTests.Orderers;
