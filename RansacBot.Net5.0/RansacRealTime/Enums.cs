namespace RansacRealTime
{
    public enum VertexType : byte
	{
		None,
		High,
		Low,
		Monkey
	}

	/// <summary>
	/// Тип сигмы.
	/// </summary>
	public enum TypeSigma : byte
	{
		ErrorThreshold,
		Sigma,
		SigmaInliers,
		СonfidenceInterval
	}

}
