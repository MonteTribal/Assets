using UnityEngine;
using System.Collections;

// A simple script to put notes in the insoector

public class Notes : MonoBehaviour {

	[TextArea(1, 20)]
	public string notes = "Notes Here";
}
