using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Stances;

public abstract class KeineStanceModel : AbstractModel
{
    private Player? _player;

    private StanceVfxController? _vfx;

    public Player Owner => _player ?? throw new InvalidOperationException("Not a mutable instance");

    private LocString Title => new("powers", "Watcher".ToUpperInvariant() + "-" + ((AbstractModel)this).Id.Entry + ".title");

    private LocString Description => new("powers", "Watcher".ToUpperInvariant() + "-" + ((AbstractModel)this).Id.Entry + ".description");

    // private string PackedIconPath => (StringExtensions.RemovePrefix(((AbstractModel)this).Id.Entry).ToLowerInvariant() + ".png").PowerImagePath();
    //
    // private Texture2D Icon => ResourceLoader.Load<Texture2D>(PackedIconPath, (string)null, (CacheMode)1);

    // public HoverTip DumbHoverTip
    // {
    // 	get
    // 	{
    // 		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
    // 		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
    // 		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
    // 		LocString description = Description;
    // 		AddDumbVariablesToDescription(description);
    // 		return new HoverTip(Title, description.GetFormattedText(), Icon);
    // 	}
    // }

    protected abstract StanceVfxConfig VfxConfig { get; }

    public IEnumerable<string> AssetPaths => VfxConfig.AssetPaths;

    public KeineStanceModel ToMutable(Player player)
    {
        var keineStanceModel = (KeineStanceModel)(object)((AbstractModel)this).MutableClone();
        keineStanceModel._player = player;
        return keineStanceModel;
    }

    private void AddDumbVariablesToDescription(LocString description)
    {
        // description.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
        // CardPoolModel val = (CardPoolModel)(object)(((AbstractModel)this).IsMutable ? ((WatcherCardPool)(object)Owner.Character.CardPool) : ModelDb.CardPool<WatcherCardPool>());
        // description.Add("energyPrefix", EnergyIconHelper.GetPrefix((AbstractModel)(object)val));
    }

    public virtual async Task OnEnterStance(PlayerChoiceContext ctx, Player owner, CardModel? source)
    {
        _vfx = new StanceVfxController(VfxConfig);
        await _vfx.OnEnter(owner.Creature);
    }

    public virtual async Task OnExitStance(PlayerChoiceContext ctx, Player owner, CardModel? source)
    {
        if (_vfx != null) await _vfx.OnExit(owner.Creature);
        _vfx = null;
    }
}