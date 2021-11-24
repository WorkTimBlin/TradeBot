namespace RansacRealTime
{
	/// <summary>
	/// Enum - Тип вершины.<br/>
	/// Undefined - Тик является вершиной.<br/>
	/// High - Вершина максимум.<br/>
	/// Low - Вершина минимум.<br/>
	/// Monkey - Вершина максимум.<br/>
	/// </summary>
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
