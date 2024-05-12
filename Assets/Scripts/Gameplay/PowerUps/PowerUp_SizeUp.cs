public sealed class PowerUp_SizeUp : PowerUp
{
	public float 	m_Factor = 1.5f;

	public override void OnPlayerTouched (Player _Player)
	{
		base.OnPlayerTouched (_Player);

		_Player.AddSizeUp (m_Factor, m_Duration);
	}
}