using RollingLineSavegameFix.Model;
using System;
using System.Text;
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
            _model.FileContent = Regex.Replace(_model.FileContent, @"(#wagons#(\s|.)*?)#", "#");                                               
        }

        public void RemoveFaultyQuickmodWaggons()
        {
            var resultBuilder = new StringBuilder();
            var match  = Regex.Match(_model.FileContent, @"(#wagons#(\s|.)*?)#");

            if (match.Success)
            {                
                var lines = match.Value.Split(Environment.NewLine);
                for (var i = 0; i < lines.Length; i++)
                {                
                    if (lines[i].StartsWith("Quickmod", StringComparison.OrdinalIgnoreCase))
                    { 
                        var values = lines[i].Split(',');
                    
                        if (Regex.IsMatch(values[0], "#|%")) //if the first value contains a # or an % its an faulty name
                            continue;

                        if (i != lines.Length - 1 && (values.Length - 1) % 32 != 0)                        
                            continue;                        

                        if (i == lines.Length - 1 && values.Length % 32 != 0)
                            continue;                                               
                    }
                    resultBuilder.Append(lines[i]);
                }

                var result = resultBuilder.ToString();
                if (!result.EndsWith("#"))
                    result += "#";

                _model.FileContent = Regex.Replace(_model.FileContent, @"(#wagons#(\s|.)*?)#", result);
            }
        }
    }
}

