using System.Numerics;

namespace AoC_2021_Day_19
{
    internal class BeaconMap
    {
        const int MIN_OVERLAP = 12;

        public readonly HashSet<Vector3> Scanners = new();
        public readonly HashSet<Vector3> Beacons = new();

        private readonly HashSet<Vector3> CurrentRotation = new();
        private readonly HashSet<Vector3> CurrentTranslation = new();

        private Vector3 TranslationOffset = new();

        static private readonly Dictionary<float, float> _radians = new()
        {
            { 0,    0 },
            { 90,   (float)Math.PI / 2 },
            { -270, (float)Math.PI / 2 },
            { 180,  (float)Math.PI },
            { -180, (float)Math.PI },
            { 270,  (float)(3 * Math.PI) / 2 },
            { -90,  (float)(3 * Math.PI) / 2 }
        };

        static private readonly List<Vector3> _rotations = new()
        {
                new(0,0,0),   new(90,0,0),   new(180,0,0),   new(270,0,0),   new(0,90,0),   new(0,-90,0),
                new(0,0,90),  new(90,0,90),  new(180,0,90),  new(270,0,90),  new(0,90,90),  new(0,-90,90),
                new(0,0,180), new(90,0,180), new(180,0,180), new(270,0,180), new(0,90,180), new(0,-90,180),
                new(0,0,270), new(90,0,270), new(180,0,270), new(270,0,270), new(0,90,270), new(0,-90,270)
        };

        public BeaconMap(List<Vector3> beaconList)
        {
            Scanners.Add(Vector3.Zero);
            Beacons.UnionWith(beaconList);
        }
        
        public bool TryAddBeaconMap(BeaconMap otherMap)
        {
            foreach (Vector3 rot in _rotations)
            {
                otherMap.RotateBeacons(rot);

                foreach (Vector3 thisBeacon in Beacons)
                {
                    TranslateBeacons(thisBeacon * -1);
                    foreach (Vector3 otherBeacon in otherMap.CurrentRotation)
                    {
                        otherMap.TranslateBeacons(otherBeacon * -1);

                        if (otherMap.CurrentTranslation.Count(this.CurrentTranslation.Contains) >= MIN_OVERLAP) //23 seconds
                        {
                            Beacons.UnionWith(otherMap.CurrentTranslation.Select(x => x - TranslationOffset));
                            Scanners.Add(otherMap.TranslationOffset - TranslationOffset);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void TranslateBeacons(Vector3 offset)
        {
            TranslationOffset = offset;
            IEnumerable<Vector3> beaconList = CurrentRotation.Count > 0 ? CurrentRotation : Beacons;

            CurrentTranslation.Clear();
            CurrentTranslation.UnionWith(beaconList.Select(x => x + offset));
        }

        private void RotateBeacons(Vector3 rotation)
        {
            Matrix4x4 rotMat = Matrix4x4.CreateRotationX(_radians[rotation.X])
                             * Matrix4x4.CreateRotationY(_radians[rotation.Y])
                             * Matrix4x4.CreateRotationZ(_radians[rotation.Z]);

            CurrentRotation.Clear();
            //round to eliminate floating point errors. 
            CurrentRotation.UnionWith(
                Beacons.Select(x => Vector3.Transform(x, rotMat))
                    .Select(x => new Vector3(
                            ((float)Math.Round(x.X)),
                            ((float)Math.Round(x.Y)),
                            ((float)Math.Round(x.Z)))));
        }

        static private int TaxiDistance(Vector3 a, Vector3 b) => (int)(Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z));

        public int GetLargestScannerDistance()
        {
            int returnValue = int.MinValue;

            List<Vector3> scannerList = Scanners.ToList();
            if (scannerList.Count < 2) return -1;

            for (int i = 0; i < scannerList.Count; i++)
            {
                for (int j = i + 1; j < scannerList.Count; j++)
                {
                    int dist = TaxiDistance(scannerList[i], scannerList[j]);
                    if (dist > returnValue) returnValue = dist;
                }
            }

            return returnValue;
        }
    }
}
