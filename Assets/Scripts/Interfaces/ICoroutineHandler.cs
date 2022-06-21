using System.Collections;

namespace Interfaces
{
    public interface ICoroutineHandler
    {
        void StartCoroutine(IEnumerator enumerator);
        void StopCoroutine(IEnumerator enumerator);
    }
}