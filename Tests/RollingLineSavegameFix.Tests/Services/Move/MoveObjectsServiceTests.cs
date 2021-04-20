using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using RollingLineSavegameFix.Model;
using RollingLineSavegameFix.Services;
using RollingLineSavegameFix.Tests.Properties;
using Xunit;

namespace RollingLineSavegameFix.Tests.Services.Move
{
    public class MoveObjectsServiceTests
    {
        [Theory, RLAutoData]
        public void Move_RegExServiceDoesNotMatch_Returns(
            [Frozen] IMainModel model,
            [Frozen] IFindObjectsRegExService findObjectsRegExService,
            [Frozen] IParseAndAddFloatValue parseAndAddFloatValue,
            MoveObjectsService sut,
            string dummyContent)
        {
            //Arrange
            model.FileContent.Returns(dummyContent);
            findObjectsRegExService.MatchRegex(dummyContent).Returns(new RegexServiceResponseModel());

            //Act
            sut.Move();

            //Assert
            model.ReceivedCalls().Should().HaveCount(1);
            findObjectsRegExService.ReceivedCalls().Should().HaveCount(1);
            findObjectsRegExService.Received(1).MatchRegex(dummyContent);
            parseAndAddFloatValue.ReceivedCalls().Should().BeEmpty();
        }

        [Theory, RLAutoData]
        public void Move_RegExMatchDoesNotContainPercentChar_ReplacesOneLine(
            [Frozen] IMainModel model,
            [Frozen] IFindObjectsRegExService findObjectsRegExService,
            [Frozen] IParseAndAddFloatValue parseAndAddFloatValue,
            MoveObjectsService sut,
            string dummyContent,
            RegexServiceResponseModel regexServiceResponseModel,
            string dummyRegex,
            float dummyX,
            float dummyY,
            float dummyZ)
        {
            //Arrange            
            var source = "table_square_0,1,16.48622_-0.2120953_61.54293,-1.347026E-05_270_-1.018759E-06,-1,1,0,0,0,-1,,-1,kcc0:h0:s0:v49:lr50:hr20,0,,";
            var expectedResult = "table_square_0,1,X_Y_Z,-1.347026E-05_270_-1.018759E-06,-1,1,0,0,0,-1,,-1,kcc0:h0:s0:v49:lr50:hr20,0,,";

            model.FileContent.Returns(dummyContent);
            model.MoveXAxisValue.Returns(dummyX);
            model.MoveYAxisValue.Returns(dummyY);
            model.MoveZAxisValue.Returns(dummyZ);
            
            regexServiceResponseModel.HasMatched = true;
            regexServiceResponseModel.Content = source;
            regexServiceResponseModel.MatchingRegEx = dummyRegex;
            findObjectsRegExService.MatchRegex(dummyContent).Returns(regexServiceResponseModel);

            parseAndAddFloatValue.For("16.48622", dummyX).Returns("X");
            parseAndAddFloatValue.For("-0.2120953", dummyY).Returns("Y");
            parseAndAddFloatValue.For("61.54293", dummyZ).Returns("Z");

            //Act
            sut.Move();

            //Assert
            model.ReceivedCalls().Should().HaveCount(6);
            findObjectsRegExService.ReceivedCalls().Should().HaveCount(2);
            findObjectsRegExService.Received(1).MatchRegex(dummyContent);
            findObjectsRegExService.Received(1).Replace(dummyContent, expectedResult, regexServiceResponseModel.MatchingRegEx);
        }

        [Theory, RLAutoData]
        public void Move_RegExMatchesMultipleObjects_ReplacesCoordinates(
            [Frozen] IMainModel model,
            [Frozen] IFindObjectsRegExService findObjectsRegExService,
            [Frozen] IMoveCoWireObjectService moveCoWireObjectService,
            [Frozen] IParseAndAddFloatValue parseAndAddFloatValue,
            MoveObjectsService sut,
            string dummyContent,
            RegexServiceResponseModel regexServiceResponseModel,
            string dummyRegex)
        {
            //Arrange            
            var source = Resources.Services_Move_MultipleObjects_Source;
            var expectedResult = Resources.Services_Move_MultipleObjects_ExpectedResult;

            model.FileContent.Returns(dummyContent);
            regexServiceResponseModel.HasMatched = true;
            regexServiceResponseModel.Content = source;
            regexServiceResponseModel.MatchingRegEx = dummyRegex;
            findObjectsRegExService.MatchRegex(dummyContent).Returns(regexServiceResponseModel);
            moveCoWireObjectService.For(Arg.Any<string>()).Returns("co_wire");


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
            findObjectsRegExService.ReceivedCalls().Should().HaveCount(2);
            findObjectsRegExService.Received(1).MatchRegex(dummyContent);
            findObjectsRegExService.Received(1).Replace(dummyContent, expectedResult, regexServiceResponseModel.MatchingRegEx);
        }
    }


    public class MoveCoWireObjectServiceTests
    {
        [Theory, RLAutoData]
        public void For_PatternDoesNotMatch_ReturnsInputString(
            [Frozen] IMainModel model,
            [Frozen] IParseAndAddFloatValue parseAndAddFloatValue,
            MoveCoWireObjectService sut,
            string dummyString)
        {
            //Arrange

            //Act
            var result = sut.For(dummyString);

            //Assert
            result.Should().Be(dummyString);
            model.ReceivedCalls().Should().BeEmpty();
            parseAndAddFloatValue.ReceivedCalls().Should().BeEmpty();
        }

        [Theory, RLAutoData]
        public void For_PatternDoesMatch_ReturnsInputStringWithNewCoordinates(
            [Frozen] IMainModel model,
            [Frozen] IParseAndAddFloatValue parseAndAddFloatValue,
            MoveCoWireObjectService sut)
        {
            //Arrange
            model.MoveXAxisValue.Returns(1);
            model.MoveYAxisValue.Returns(2);
            model.MoveZAxisValue.Returns(3);

            var input = "14,co_wire,1,12.68545_2.453807_61.43925,3.329947_138.1346_1.069022E-07,-1,0.5,0,0,0,-1,12.68545_2.453807_61.43925&13.11526_2.416335_60.95963&0.5&2,-1,kcc0:h0:s0:v26:lr50:hr20,0,,%14,";
            var expectedResult = "14,co_wire,1,X_Y_Z,3.329947_138.1346_1.069022E-07,-1,0.5,0,0,0,-1,X_Y_Z&A_B_C&0.5&2,-1,kcc0:h0:s0:v26:lr50:hr20,0,,%14,";


            parseAndAddFloatValue.For("12.68545", 1).Returns("X");
            parseAndAddFloatValue.For("2.453807", 2).Returns("Y");
            parseAndAddFloatValue.For("61.43925", 3).Returns("Z");
            
            parseAndAddFloatValue.For("13.11526", 1).Returns("A");
            parseAndAddFloatValue.For("2.416335", 2).Returns("B");
            parseAndAddFloatValue.For("60.95963", 3).Returns("C");

            //Act
            var result = sut.For(input);

            //Assert
            result.Should().Be(expectedResult);            
        }




    }
}
