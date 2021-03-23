﻿using RollingLineSavegameFix.Model;
using System;

namespace RollingLineSavegameFix.Services
{
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
            content = content.Replace("QuickMod", $"{Environment.NewLine}QuickMod", StringComparison.OrdinalIgnoreCase);
            _model.FileContent = content;
        }
    }
}
