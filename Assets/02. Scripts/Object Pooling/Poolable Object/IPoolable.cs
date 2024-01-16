using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    // �Ҵ�� ������Ʈ Ǯ
    public ObjectPool pool { get; set; }

    /// <summary>
    /// ������Ʈ�� ���� �������� �� ������ �޼���
    /// </summary>
    public abstract void OnCreate();
    /// <summary>
    /// Ǯ���� ������Ʈ�� Ȱ��ȭ ���� �� ������ �޼���
    /// </summary>
    /// <param name="pool">�Ҵ�� Ǯ</param>
    public abstract void OnActivate();
    /// <summary>
    /// ������Ʈ�� Ǯ�� ��ȯ�� �� ������ �޼���
    /// </summary>
    public abstract void ReturnObject();
}