using System.Collections.Generic;
using TileService.Models.Geometry;
using Xunit;

namespace TileService.Tests
{
    public class Roads
    {
        static readonly Tile Tile = new Tile(18, 0, 0);
        static readonly Point[] StraightWayPoints = {
            new Point(0, 0),
            new Point(0, 1),
        };

        IDictionary<string, string> GetTagsFromList(IList<string> list)
        {
            var dict = new Dictionary<string, string>(list.Count);
            for (var index = 0; index < list.Count; index++) {
                var split = list[index].Split('=', 2);
                dict.Add(split[0], split[1]);
            }
            return dict;
        }

        Way GetStraightWay(IDictionary<string, string> tags)
        {
            return new Way(Tile, tags, StraightWayPoints);
        }

        Road GetStraightRoad(IDictionary<string, string> tags)
        {
            return GetStraightWay(tags).Road;
        }

        string GetStraightRoadText(params string[] tags)
        {
            return GetStraightRoad(GetTagsFromList(tags)).ToString();
        }

        [Fact]
        public void HighwayRoad()
        {
            Assert.Equal(
                "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Edge 0.0m, Center=3.0m)",
                GetStraightRoadText("highway=road")
            );
        }

        [Fact]
        public void HighwayRoadLanes1()
        {
            Assert.Equal(
                "Road(Edge 0.0m|Car 3.0m|Edge 0.0m, Center=1.5m)",
                GetStraightRoadText("highway=road", "lanes=1")
            );
        }

        [Fact]
        public void HighwayRoadLanes2()
        {
            Assert.Equal(
                "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Edge 0.0m, Center=3.0m)",
                GetStraightRoadText("highway=road", "lanes=2")
            );
        }

        [Fact]
        public void HighwayRoadLanes3()
        {
            Assert.Equal(
                "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Car 3.0m|Edge 0.0m, Center=4.5m)",
                GetStraightRoadText("highway=road", "lanes=3")
            );
        }

        [Fact]
        public void HighwayRoadLanes4()
        {
            Assert.Equal(
                "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Car 3.0m|Car 3.0m|Edge 0.0m, Center=6.0m)",
                GetStraightRoadText("highway=road", "lanes=4")
            );
        }

        [Fact]
        public void HighwayRoadOnewayYes()
        {
            Assert.Equal(
                "Road(Edge 0.0m|Car 3.0m|Edge 0.0m, Center=1.5m)",
                GetStraightRoadText("highway=road", "oneway=yes")
            );
        }

        [Fact]
        public void HighwayRoadOnewayYesLanes1()
        {
            Assert.Equal(
                "Road(Edge 0.0m|Car 3.0m|Edge 0.0m, Center=1.5m)",
                GetStraightRoadText("highway=road", "oneway=yes", "lanes=1")
            );
        }

        [Fact]
        public void HighwayRoadOnewayYesLanes2()
        {
            Assert.Equal(
                "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Edge 0.0m, Center=3.0m)",
                GetStraightRoadText("highway=road", "oneway=yes", "lanes=2")
            );
        }

        [Fact]
        public void HighwayRoadOnewayYesLanes3()
        {
            Assert.Equal(
                "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Car 3.0m|Edge 0.0m, Center=4.5m)",
                GetStraightRoadText("highway=road", "oneway=yes", "lanes=3")
            );
        }

        [Fact]
        public void HighwayRoadOnewayYesLanes4()
        {
            Assert.Equal(
                "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Car 3.0m|Car 3.0m|Edge 0.0m, Center=6.0m)",
                GetStraightRoadText("highway=road", "oneway=yes", "lanes=4")
            );
        }

        [Fact]
        public void WikiBicycleExampleL1a()
        {
            const string road = "Road(Edge 0.0m|Cycle 1.0m|Car 3.0m|Car 3.0m|Cycle 1.0m|Edge 0.0m, Center=4.0m)";
            Assert.Equal(road, GetStraightRoadText("highway=road", "cycleway=lane"));
            Assert.Equal(road, GetStraightRoadText("highway=road", "cycleway:left=lane", "cycleway:right=lane"));
            Assert.Equal(road, GetStraightRoadText("highway=road", "cycleway:both=lane"));
        }

        [Fact]
        public void WikiBicycleExampleL1b()
        {
            const string road = "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Cycle 1.0m|Cycle 1.0m|Edge 0.0m, Center=3.0m)";
            Assert.Equal(road, GetStraightRoadText("highway=road", "cycleway:right=lane", "cycleway:right:oneway=no"));
            // Can't be distinguished from L1a: Assert.Equal(road, GetStraightRoadText("highway=road", "cycleway=lane"));
        }

        [Fact]
        public void WikiBicycleExampleL2()
        {
            const string road = "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Cycle 1.0m|Edge 0.0m, Center=3.0m)";
            Assert.Equal(road, GetStraightRoadText("highway=road", "cycleway:right=lane"));
        }

        [Fact]
        public void WikiBicycleExampleM1()
        {
            const string road = "Road(Edge 0.0m|Cycle 1.0m|Car 3.0m|Cycle 1.0m|Edge 0.0m, Center=2.5m)";
            Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "cycleway=lane", "oneway:bicycle=no"));
            Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "cycleway:left=opposite_lane", "cycleway:right=lane"));
        }

        [Fact]
        public void WikiBicycleExampleM2a()
        {
            const string road = "Road(Edge 0.0m|Car 3.0m|Cycle 1.0m|Edge 0.0m, Center=1.5m)";
            Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "cycleway:right=lane"));
            // Can't be distinguished from M2b: Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "cycleway=lane"));
        }

        [Fact]
        public void WikiBicycleExampleM2b()
        {
            const string road = "Road(Edge 0.0m|Cycle 1.0m|Car 3.0m|Edge 0.0m, Center=2.5m)";
            Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "cycleway:left=lane"));
            // Can't be distinguished from M2a: Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "cycleway=lane"));
        }

        [Fact]
        public void WikiBicycleExampleM2c()
        {
            const string road = "Road(Edge 0.0m|Car 3.0m|Cycle 1.0m|Car 3.0m|Edge 0.0m, Center=3.5m)";
            // Can't be distinguished from L1a: Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "lanes=2", "cycleway=lane"));
        }

        [Fact]
        public void WikiBicycleExampleM2d()
        {
            const string road = "Road(Edge 0.0m|Cycle 1.0m|Cycle 1.0m|Car 3.0m|Edge 0.0m, Center=3.5m)";
            Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "oneway:bicycle=no", "cycleway:left=lane", "cycleway:left:oneway=no"));
        }

        [Fact]
        public void WikiBicycleExampleM3a()
        {
            const string road = "Road(Edge 0.0m|Cycle 1.0m|Car 3.0m|Edge 0.0m, Center=2.5m)";
            Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "oneway:bicycle=no", "cycleway:left=opposite_lane"));
            // Can't be distinguished from M3b: Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "oneway:bicycle=no", "cycleway=opposite_lane"));
        }

        [Fact]
        public void WikiBicycleExampleM3b()
        {
            const string road = "Road(Edge 0.0m|Car 3.0m|Cycle 1.0m|Edge 0.0m, Center=1.5m)";
            Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "oneway:bicycle=no", "cycleway:right=opposite_lane"));
            // Can't be distinguished from M3a: Assert.Equal(road, GetStraightRoadText("highway=road", "oneway=yes", "oneway:bicycle=no", "cycleway=opposite_lane"));
        }

        // [Fact]
        // public void WikiBicycleExampleT1MultiWay1()
        // {
        //     Assert.Equal(
        //         "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Edge 0.0m, Center=3.0m)",
        //         GetStraightRoadText("highway=road", "bicycle=use_sidepath")
        //     );
        // }

        // [Fact]
        // public void WikiBicycleExampleT1MultiWay2()
        // {
        //     Assert.Equal(
        //         "Road(Edge 0.0m|Cycle 1.0m|Edge 0.0m, Center=0.5m)",
        //         GetStraightRoadText("highway=cycleway", "oneway=yes")
        //     );
        // }

        // [Fact]
        // public void WikiBicycleExampleT1SingleWay()
        // {
        //     Assert.Equal(
        //         "Road(Edge 0.0m|Cycle 1.0m|Verge 0.5m|Car 3.0m|Car 3.0m|Verge 0.5m|Cycle 1.0m|Edge 0.0m, Center=4.5m)",
        //         GetStraightRoadText("highway=road", "cycleway=track")
        //     );
        // }

        // // WikiBicycleExampleT2MultiWay1 would be identical to WikiBicycleExampleT1MultiWay1

        // [Fact]
        // public void WikiBicycleExampleT2MultiWay2()
        // {
        //     Assert.Equal(
        //         "Road(Edge 0.0m|Cycle 1.0m|Cycle 1.0m|Edge 0.0m, Center=1.0m)",
        //         GetStraightRoadText("highway=cycleway", "oneway=no")
        //     );
        // }

        // [Fact]
        // public void WikiBicycleExampleT2SingleWay()
        // {
        //     Assert.Equal(
        //         "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Verge 0.5m|Cycle 1.0m|Cycle 1.0m|Edge 0.0m, Center=3.0m)",
        //         GetStraightRoadText("highway=road", "cycleway:right=track", "cycleway:right:oneway=no")
        //     );
        // }

        // [Fact]
        // public void WikiBicycleExampleT3MultiWay1()
        // {
        //     Assert.Equal(
        //         "Road(Edge 0.0m|Car 3.0m|Edge 0.0m, Center=1.5m)",
        //         GetStraightRoadText("highway=road", "oneway=yes", "bicycle=use_sidepath")
        //     );
        // }

        // // WikiBicycleExampleT3MultiWay2 would be identical to WikiBicycleExampleT2MultiWay2

        // [Fact]
        // public void WikiBicycleExampleT3SingleWay()
        // {
        //     Assert.Equal(
        //         "Road(Edge 0.0m|Car 3.0m|Verge 0.5m|Cycle 1.0m|Cycle 1.0m|Edge 0.0m, Center=1.5m)",
        //         GetStraightRoadText("highway=road", "oneway=yes", "cycleway:right=track", "oneway:bicycle=no")
        //     );
        // }

        // // WikiBicycleExampleT4MultiWay1 would be identical to WikiBicycleExampleT1MultiWay1

        // // WikiBicycleExampleT4MultiWay2 would be identical to WikiBicycleExampleT1MultiWay2

        // [Fact]
        // public void WikiBicycleExampleT4SingleWay()
        // {
        //     Assert.Equal(
        //         "Road(Edge 0.0m|Car 3.0m|Car 3.0m|Verge 0.5m|Cycle 1.0m|Edge 0.0m, Center=3.0m)",
        //         GetStraightRoadText("highway=road", "cycleway:right=track")
        //     );
        // }
    }
}