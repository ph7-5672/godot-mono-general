
namespace GodotMonoGeneral.Logic;

public interface ISparseSet
{
    void Delete(int entityId);

    SparseSnapshot GetSnapshot();

    void LoadSnapshot(SparseSnapshot snapshot);

}