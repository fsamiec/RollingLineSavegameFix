using Xunit;
using FluentAssertions;
using RollingLineSavegameFix.Services;
using RollingLineSavegameFix.Model;
using System;
using NSubstitute;

namespace RollingLineSavegameFix.Tests.Services
{
    public class ReformatServiceTests
    {
        [Theory, RLAutoData]
        public void Reformat_ServiceFindsNothingToReformat_DoesNothing(
            IMainModel mainModel, 
            ReformatService sut,
            string dummyContent)
        {
            //Arrange
            var unmodified = dummyContent;
            mainModel.FileContent.Returns(dummyContent);

            //Act
            sut.Reformat();

            //Assert
            mainModel.FileContent.Should().Be(unmodified);
        }

        [Theory, RLAutoData]
        public void Reformat_ServiceFindsNoQuickMods_EliminatesAllLineBreaks(
            IMainModel mainModel,
            ReformatService sut,            
            int randomLineBreakInserter
            )
        {
            //Arrange
            var lorizzleIppsle = "Lorizzle ipsum dolor ghetto amizzle, mah nizzle adipiscing elit. You son of a bizzle ut dolizzle.Things magna ligula, dignissim sit amizzle, that's the shizzle eget, the bizzle nec, the bizzle.";
            var content = lorizzleIppsle;
            mainModel.FileContent.Returns(content);
            randomLineBreakInserter = randomLineBreakInserter % 5 + 2;

            mainModel.FileContent = lorizzleIppsle;
            var lineBreakCounter = 0;
            for (var i = randomLineBreakInserter; i < mainModel.FileContent.Length; i = i + randomLineBreakInserter + 1)
            {
                mainModel.FileContent.Insert(i, Environment.NewLine);
                lineBreakCounter++;
            }

            //Act
            sut.Reformat();
            
            //Assert
            mainModel.FileContent.Should().Be(lorizzleIppsle);
            lineBreakCounter.Should().NotBe(0);
        }
    }
}
