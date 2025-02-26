using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SteelStorm : MonoBehaviour
{
    public Entity Parent; //누가 쐈는지

    private void OnTriggerEnter(Collider other)
    {
        Parent = transform.parent.GetComponent<ObjectMove>().Attacker;
        
        // 몬스터가 공격 시
        if (other.gameObject.CompareTag("Player") && Parent.CompareTag("Enemy"))  // 회오리가 캐릭터에 맞고 Attacker가 Enemy 이면
        {
            // 캐릭터 공중에 띄움
            StartCoroutine(UpPlayer(other));

            // 캐릭터에게 데미지 줌
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
        // 캐릭터가 공격 시
        else if (other.gameObject.CompareTag("Enemy") && Parent.CompareTag("Player"))  // 회오리가 몬스터에 맞고 Attacker가 Player 이면
        {
            if (other.gameObject.name != "Archer(Clone)" && other.gameObject.name != "eyebat(Clone)" && other.gameObject.name != "Mad Flower(Clone)")
            {
                // 몬스터 공중에 띄움
                StartCoroutine(UpPlayer(other));
            }
            else
            {
                Debug.Log(other.name + "공중에 안띄움");
            }
            
            // 몬스터에게 데미지
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
    }
    IEnumerator UpPlayer(Collider other)
    {
        Debug.Log(other.name + " move UP");

        // 캐릭터 위로 띄움
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 1000f, ForceMode.Force);
        // 몬스터 띄움
        else
        {
            // 몬스터의 내비게이션 비활성화
            other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 300f, ForceMode.Force);
        }

        // 못움직이게 Position X,Y 프리즈
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        yield return new WaitForSeconds(1f);

        if(other.gameObject.CompareTag("Enemy"))
            other.gameObject.GetComponent<NavMeshAgent>().enabled = true;

        // 1초 뒤 프리즈 해제 및 Rotation X,Z 만 다시 잠금
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    
    }
}
