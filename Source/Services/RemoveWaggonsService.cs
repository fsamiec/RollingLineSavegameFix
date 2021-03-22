using RollingLineSavegameFix.Model;
using System;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public class RemoveWaggonsService : IRemoveWaggonsService
    {
        private readonly MainModel _model;

        public RemoveWaggonsService(MainModel model)
        {
            _model = model;
        }

        public void RemoveAllWaggons()
        {            
            var start = DateTime.Now;
            _model.FileContent = Regex.Replace(_model.FileContent, @"(#wagons#(\s|.)*?)#", "#");                                   
            var finish = DateTime.Now;

            var elapsed = finish - start;
            Console.WriteLine($"This shit took {elapsed.TotalSeconds} seconds");
        }

        public void RemoveFaultyQuickmodWaggons()
        {
            throw new NotImplementedException();
        }
    }
}

