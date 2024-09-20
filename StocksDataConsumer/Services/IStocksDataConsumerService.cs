using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksDataConsumer.Services;

public interface IStocksDataConsumerService
{
    Task StartListening();
}
