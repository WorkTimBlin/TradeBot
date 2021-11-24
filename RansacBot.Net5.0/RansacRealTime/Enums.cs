namespace RansacRealTime
{
	/// <summary>
	/// Enum - Тип вершины.<br/>
	/// Undefined - Тик является вершиной.<br/>
	/// High - Вершина максимум.<br/>
	/// Low - Вершина минимум.<br/>
	/// Monkey - Промежуточная вершина Monkey.<br/>
	/// </summary>
	public enum VertexType : byte
	{

		High,
		/// <summary>
		/// Вершина минимум.
		/// </summary>
		Low,
		/// <summary>
		/// Промежуточная вершина Monkey.
		/// </summary>
		Monkey
	}

	/// <summary>
	/// Enum - Тип сигмы.<br/>
	/// ErrorThreshold - Ошибка по инлайнерам.<br/>
	/// Sigma - Обычная сигма.<br/>
	/// SigmaInliers - Обычная сигма, посчитанная исключительно на точках-инлайнерах.<br/>
	/// СonfidenceInterval - Доверительный интервал, основанный на Percentile.<br/>
	/// </summary>
	public enum TypeSigma : byte
	{
		/// <summary>
		/// Ошибка по инлайнерам.
		/// </summary>
		ErrorThreshold,
		/// <summary>
		/// Обычная сигма.
		/// </summary>
		Sigma,
		/// <summary>
		/// Обычная сигма, посчитанная исключительно на точках-инлайнерах.
		/// </summary>
		SigmaInliers,
		/// <summary>
		/// Доверительный интервал, основанный на Percentile.
		/// </summary>
		СonfidenceInterval
	}

	/// <summary>
	/// Enum - Действие при добавлении новой вершины в ранзак. <br/>
	/// Add - Добавить вершину.<br/>
	/// Rebuild - Перестроить ранзак.<br/>
	/// Stop - Закончить ранзак.<br/>
	/// Nothing - Ничего не делать.<br/>
	/// </summary>
	public enum Action : byte
	{
		/// <summary>
		/// Добавить вершину.
		/// </summary>
		Add,
		/// <summary>
		/// Перестроить ранзак.
		/// </summary>
		Rebuild,
		/// <summary>
		/// Закончить ранзак.
		/// </summary>
		Stop,
		/// <summary>
		/// Ничего не делать.
		/// </summary>
		Nothing
	}
}
