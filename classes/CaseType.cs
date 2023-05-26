using System;
using System.Collections.Generic;

public class CaseType
{
    public string Name { get; set; }
    public int Value { get; set; }
    public CaseType()
    {
        // Create a linked list of case types
        LinkedList<CaseType> caseTypes = new LinkedList<CaseType>();

        // Add case types to the linked list
        caseTypes.AddLast(new CaseType { Name = "Civil", Value = 1 });
        caseTypes.AddLast(new CaseType { Name = "Criminal", Value = 2 });
        caseTypes.AddLast(new CaseType { Name = "Family", Value = 3 });
        caseTypes.AddLast(new CaseType { Name = "Business", Value = 4});
        caseTypes.AddLast(new CaseType { Name = "Personal Injury", Value = 5 });
        caseTypes.AddLast(new CaseType { Name = "Real Estate", Value = 6 });
        caseTypes.AddLast(new CaseType { Name = "Bankruptcy", Value = 7 });
        caseTypes.AddLast(new CaseType { Name = "Employment", Value = 8 });
        caseTypes.AddLast(new CaseType { Name = "Intellectual Property", Value = 9 });
        // this list is longer, see design 
        // Populate a dropdown list with the case types
        foreach (CaseType caseType in caseTypes)
        {
            // Create a new dropdown list item using the case type's name and value
            ListItem item = new ListItem(caseType.Name, caseType.Value.ToString());
            
            // Add the item to the dropdown list
            ddlCaseType.Items.Add(item);
        }
    }
}

}
    
