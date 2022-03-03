namespace RansacsRealTime
{
	public enum VertexType : byte
	{
		High,
		Low,
		Monkey
	}

	public enum SigmaType : byte
	{
		ErrorThreshold = 0,
		Sigma = 1,
		SigmaInliers = 2,
		СonfidenceInterval = 3
	}
}
