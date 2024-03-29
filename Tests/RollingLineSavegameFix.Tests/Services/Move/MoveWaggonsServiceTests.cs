﻿using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using RollingLineSavegameFix.Model;
using RollingLineSavegameFix.Services;
using RollingLineSavegameFix.Tests.Properties;
using Xunit;

namespace RollingLineSavegameFix.Tests.Services.Move
{
    public class MoveWaggonsServiceTests
    {  

        [Theory, RLAutoData]
        public void Constructor_ReturnsI(MoveWaggonsService sut)
        {
            sut.Should().BeAssignableTo<IMoveWaggonsService>();
            sut.Should().BeAssignableTo<MoveSomethingService>();
        }

        [Theory, RLAutoData]
        public void Move_RegExServiceDoesNotMatch_Returns(
            [Frozen] IMainModel model,
            [Frozen] IFindWaggonsRegExService findWaggonsRegExService,
            [Frozen] IParseAndAddFloatValue parseAndAddFloatValue,
            MoveWaggonsService sut,
            string dummyContent)
        {
            //Arrange
            model.FileContent.Returns(dummyContent);
            findWaggonsRegExService.MatchRegex(dummyContent).Returns(new RegexServiceResponseModel());

            //Act
            sut.Move();

            //Assert
            model.ReceivedCalls().Should().HaveCount(1);
            findWaggonsRegExService.ReceivedCalls().Should().HaveCount(1);
            findWaggonsRegExService.Received(1).MatchRegex(dummyContent);
            parseAndAddFloatValue.ReceivedCalls().Should().BeEmpty();
        }

        [Theory, RLAutoData]
        public void Move_RegExMatchDoesNotContainPercentChar_ReplacesOneLine(
            [Frozen] IMainModel model,
            [Frozen] IFindWaggonsRegExService findWaggonsRegExService,
            [Frozen] IParseAndAddFloatValue parseAndAddFloatValue,
            MoveWaggonsService sut,
            string dummyContent,
            RegexServiceResponseModel regexServiceResponseModel,
            string dummyRegex)
        {
            //Arrange            
            var source = "ct_dynamic,1,16.73281_1.918186_60.32922,0_125.4218_0,43,36,42,0,0,0,0,0,0,0,0,0,0,0,-1,0,,1_0_-2_45_2.9_0_6_22_1_1_12,-1,0,4_0,kcc0:h0:s0:v49:lr50:hr20,,1,";
            var expectedResult = "ct_dynamic,1,X_Y_Z,0_125.4218_0,43,36,42,0,0,0,0,0,0,0,0,0,0,0,-1,0,,1_0_-2_45_2.9_0_6_22_1_1_12,-1,0,4_0,kcc0:h0:s0:v49:lr50:hr20,,1,";

            model.FileContent.Returns(dummyContent);
            regexServiceResponseModel.HasMatched = true;
            regexServiceResponseModel.Content = source;
            regexServiceResponseModel.MatchingRegEx = dummyRegex;
            findWaggonsRegExService.MatchRegex(dummyContent).Returns(regexServiceResponseModel);

            parseAndAddFloatValue.For("16.73281", Arg.Any<float>()).Returns("X");
            parseAndAddFloatValue.For("1.918186", Arg.Any<float>()).Returns("Y");
            parseAndAddFloatValue.For("60.32922", Arg.Any<float>()).Returns("Z");

            //Act
            sut.Move();

            //Assert
            model.ReceivedCalls().Should().HaveCount(6);
            findWaggonsRegExService.ReceivedCalls().Should().HaveCount(2);
            findWaggonsRegExService.Received(1).MatchRegex(dummyContent);
            findWaggonsRegExService.Received(1).Replace(dummyContent, expectedResult, regexServiceResponseModel.MatchingRegEx);
        }

        [Theory, RLAutoData]
        public void Move_RegExMatchesMultipleWaggons_ReplacesCoordinates(
            [Frozen] IMainModel model,
            [Frozen] IFindWaggonsRegExService findWaggonsRegExService,
            [Frozen] IParseAndAddFloatValue parseAndAddFloatValue,
            MoveWaggonsService sut,
            string dummyContent,
            RegexServiceResponseModel regexServiceResponseModel,
            string dummyRegex)
        {
            //Arrange            
            var source = Resources.Services_Move_MultipleWaggons_Source;
            var expectedResult = Resources.Services_Move_MultipleWaggons_ExpectedResult;

            model.FileContent.Returns(dummyContent);
            regexServiceResponseModel.HasMatched = true;
            regexServiceResponseModel.Content = source;
            regexServiceResponseModel.MatchingRegEx = dummyRegex;
            findWaggonsRegExService.MatchRegex(dummyContent).Returns(regexServiceResponseModel);

            var coordinateCounter = 0;
            parseAndAddFloatValue.For(Arg.Any<string>(), Arg.Any<float>()).Returns((ci) =>
            {
                switch (coordinateCounter)
                {
                    case 0:
                        coordinateCounter++;
                        return "X";
                    case 1:
                        coordinateCounter++;
                        return "Y";
                    default:
                        coordinateCounter = 0;
                        return "Z";
                };
            });

            //Act
            sut.Move();

            //Assert            
            findWaggonsRegExService.ReceivedCalls().Should().HaveCount(2);
            findWaggonsRegExService.Received(1).MatchRegex(dummyContent);
            findWaggonsRegExService.Received(1).Replace(dummyContent, expectedResult, regexServiceResponseModel.MatchingRegEx);
        }


    }

}
