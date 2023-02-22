using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class NodeGraph : MonoBehaviour
{
    public static NodeGraph instance = null;

    private EditorManager editorMgr;

    public Node head;

    private GameObject nodePref;

    public Node selectedNode = null;

    private RectTransform rect;

    private Stack<EditorCommand> commands = new(); //����� Ŀ�ǵ�
    private Stack<EditorCommand> redoCommands = new(); //�𵵵� Ŀ�ǵ�

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        editorMgr = EditorManager.instance;

        nodePref = Resources.Load<GameObject>("Prefabs/ScriptEditor/Node");
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        transform.localPosition = new Vector2(-150, -50);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
            .Subscribe(_ =>
            {
                EditorCommand cmd;
                
                if(commands.TryPop(out cmd) == true)
                {
                    cmd.Undo();
                    redoCommands.Push(cmd);
                }
            });

        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Y))
            .Subscribe(_ =>
            {
                EditorCommand cmd;

                if (redoCommands.TryPop(out cmd) == true)
                {
                    cmd.Execute();
                    commands.Push(cmd);
                }
            });

        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.C) && Input.GetMouseButtonDown(0))
            .Subscribe(_ => CreateNextNode());

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.B))
            .Subscribe(_ => CreateBranch());

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Delete))
            .Subscribe(_ => RemoveNode());

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.S))
            .Subscribe(_ => Save());

        Observable.EveryUpdate()
           .Where(_ => Input.GetKeyDown(KeyCode.E))
           .Subscribe(_ =>
           {
               if (selectedNode.scriptType == Node.ScriptType.Text)
               {
                   selectedNode.scriptType = Node.ScriptType.Event;
               }
               else
               {
                   selectedNode.scriptType = Node.ScriptType.Text;
               }

               Save();
           });


        SetNodePosition();
        SetContentSize();
        SelectNode(selectedNode ?? head);
    }

    public void Save()
    {
        ScriptInspector.instance.ApplyInspector();
        ScriptInspector.instance.SetInspector(selectedNode);

        PrefabUtility.SaveAsPrefabAsset(this.gameObject, Application.dataPath + "/Resources/Prefabs/ScriptGraph/ScriptGraph" + editorMgr.scriptGroupID + ".prefab");
        "Save".Log();
    }

    public void CreateGraph()
    {
        int scriptGroupID = editorMgr.scriptGroupID;

        Node node = Instantiate(nodePref).GetComponent<Node>();

        node.transform.SetParent(transform);
        node.transform.localScale = Vector3.one;
        node.transform.localPosition = Vector3.zero;

        head = node;

        if (selectedNode == null)
        {
            SelectNode(node);
        }
    }

    public Node CreateNode()
    {
        Node node = Instantiate(nodePref).GetComponent<Node>();

        node.transform.SetParent(transform);
        node.transform.localScale = Vector3.one;

        return node;
    }

    #region commands
    public void CreateBranch()
    {
        if (selectedNode.nodeType == Node.NodeType.Goto)
        {
            return;
        }

        if (selectedNode.nodeType == Node.NodeType.BranchEnd)
        {
            return;
        }

        if (selectedNode.branch[Node.maxBranchCount - 1] != null)
        {
            return;
        }

        EditorCommand command = new CreateBranchNode();
        command.Execute();
        commands.Push(command);
    }

    public void CreateNextNode()
    {
        if (selectedNode.nodeType == Node.NodeType.Goto)
        {
            return;
        }

        if(selectedNode.nodeType == Node.NodeType.BranchEnd)
        {
            return;
        }

        EditorCommand command = new CreateNextNode();
        command.Execute();
        commands.Push(command);
    }

    public void RemoveNode()
    {
        if (selectedNode.nextNode == null && selectedNode.prevNode == null) //������ �ϳ� ���� ����� ������ ����
        {
            return;
        }

        if(selectedNode.parent != null)
        {
            EditorCommand command = new RemoveBranchNode();
            command.Execute();
            commands.Push(command);
        }
        else
        {
            EditorCommand command = new RemoveNode();
            command.Execute();
            commands.Push(command);
        }


    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="includeBranch"></param>
    /// <param name="action">index, branchIndex, depth, Node<br></br><br></br>
    /// index : �θ� ���� ���鸸 �������� ��, �ش� ����� ��ȣ<br></br>
    /// branchIndex : �ش� �귣ġ�� ���� �θ��� branch�� index null<br></br>
    /// depth : �ش� �귣ġ�� ���� ���鸸 �������� ��, �ش� ����� �ش� �귣ġ������ ��ȣ. �θ� ���ٸ� null
    /// </param>
    /// <returns></returns>
    public int TraversalNode(bool includeBranch, Node head, Action<int, int?, int, Node> action, int index = 0)
    {
        Node node = head;
        int? branchIndex = null;
        int depth = 0;

        while(node != null)
        {
            ++index;
            ++depth;
            branchIndex = node.GetBranchIndex() != -1 ? node.GetBranchIndex() : null;

            if (action != null)
            {
                action(index, branchIndex, depth, node);
            }

            if(includeBranch == true)
            {
                if (node.branch.Count > 0)
                {
                    foreach (var branch in node.branch)
                    {
                        index = TraversalNode(true, branch, action, index);
                    }
                }
            }

            node = node.nextNode;
        }

        return index;
    }

    public int GetNodeCount()
    {
        return TraversalNode(false, head, null);
    }

    public void SetNodePosition()
    {
        int loopCount = TraversalNode(true, head, (index, branchIndex, depth, node) =>
        {
            node.SetScriptID(index);

            string name = "";

            if(node.parent == null)
            {
                name += depth.ToString();
            }
            else
            {
                if (branchIndex != null)
                {
                    name += node.parent.name;
                    name += " : " + branchIndex;
                    name += " - " + depth;
                }
            }
            
            node.name = name;
            node.SetText(name);

            if (node.isHead == true)
            {
                if (node.parent == null)
                {
                    node.transform.localPosition = Vector2.zero;
                }
                else if (branchIndex != null && node.isHead == true)
                {
                    Vector2 pos = node.parent.transform.localPosition;
                    pos.y += Node.interval.y;
                    pos.x += (Node.interval.x * (branchIndex + 1) ?? 1) - Node.interval.x * 0.5f;

                    node.transform.localPosition = pos;
                }
            }
            else if(node.prevNode != null)
            {

                Vector2 pos = node.prevNode.transform.localPosition;

                int childCount = node.prevNode.GetChildCount() + 1;
                pos.y += Node.interval.y * childCount;
                node.transform.localPosition = pos;
            }
        });
    }

    public void SetContentSize()
    {
        int nodeCount = GetNodeCount();

        Vector2 sizeDelta = rect.sizeDelta;
        sizeDelta.y = nodeCount * Mathf.Abs(Node.interval.y) + 100;
        rect.sizeDelta = sizeDelta;

        editorMgr.scrollViewContent.sizeDelta = new Vector2(editorMgr.scrollViewContent.sizeDelta.x, sizeDelta.y);
    }

    #region ��� ����
    public void SelectNode(Node node)
    {
        if(node.nodeType == Node.NodeType.BranchEnd)
        {
            "�귣ġ�� ���� ��带 �߰��� �� �����ϴ�.".Log();
            return;
        }

        selectedNode?.Deselect();

        selectedNode = node;
        node.Select();
        ScriptInspector.instance.SetInspector(node);
    }
    #endregion
}
