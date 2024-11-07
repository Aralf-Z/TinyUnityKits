using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using RedSaw.CommandLineInterface;
using UnityEngine;

public class CommandCreator:
    ICommandCreator
{
    public Type[] GetAssemblyTypes() => Assembly.GetExecutingAssembly().GetTypes();
}