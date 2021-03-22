using RegexMatcher;
using RollingLineSavegameFix.Model;
using System;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    /// <summary>
    /// Service to manipulate Savegames
    /// </summary>
    public interface ISavegameService
    {
        /// <summary>
        /// Loads a Savegame
        /// </summary>
        /// <returns>Error Message trying to read the file</returns>
        string LoadSavegame();

        /// <summary>
        /// Tries to Write a new Savefile
        /// </summary>
        /// <returns>Filename to new Savegame</returns>
        void WriteNewSaveGame();
    }

    public interface IReformatService
    {
        void Reformat();
    }

    public class ReformatService : IReformatService
    {
        private readonly MainModel _model;

        public ReformatService(MainModel model)
        {
            _model = model;
        }

        public void Reformat()
        {
            var content = _model.FileContent;
            content = content.Replace(Environment.NewLine, "");
            content = content.Replace("Quickmod", $"{Environment.NewLine}Quickmod");
            _model.FileContent = content;
        }
    }

    public interface IRemoveWaggonsService
    {
        void RemoveAllWaggons();
        void RemoveFaultyQuickmodWaggons();
    }

    public class RemoveWaggonsService : IRemoveWaggonsService
    {
        private readonly MainModel _model;

        public RemoveWaggonsService(MainModel model)
        {
            _model = model;
        }

        public void RemoveAllWaggons()
        {

            Matcher matcher = new Matcher();
            var regex = new Regex("(#wagons#(\\s|.)*?)#/g");
            matcher.Add(regex, "bla");
            

            if (matcher.Match(_model.FileContent, out var x))
            {
                Console.WriteLine(x);
            }

            var start = DateTime.Now;
            
            if (regex.IsMatch(_model.FileContent))
            {
                Console.WriteLine("JAWOHL");
            }
            var bla = Regex.Replace(_model.FileContent, "(#wagons#(\\s|.)*?)#/g", "");

            _model.FileContent = regex.Replace(_model.FileContent, "#");
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

