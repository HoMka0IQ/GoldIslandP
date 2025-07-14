using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public interface BuildRules
{
    public bool AcceptRule(Cell_SO cell_SO);

    public string RuleDescription();

}
