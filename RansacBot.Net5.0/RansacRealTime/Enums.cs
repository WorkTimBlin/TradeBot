namespace RansacRealTime
{
	/// <summary>
	/// Тип вершины(экстремума) MonkeyN.
	/// </summary>
    public enum VertexType : byte
	{
		
		Undefined,
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
