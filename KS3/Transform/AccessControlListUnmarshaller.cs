using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using KS3.Model;

namespace KS3.Transform
{
    public class AccessControlListUnmarshaller : Unmarshaller<AccessControlList, Stream>
    {
        public AccessControlList unmarshall(Stream inputStream)
        {
            //Console.WriteLine((new StreamReader(inputStream)).ReadToEnd());
            //return null;

            AccessControlList acl = new AccessControlList();
            Owner owner = null;
            String ownerId = null;
            String ownerDisplayName = null;
            Grantee grantee = null;
            String granteeType = null;
            String userId = null;
            String userDisplayName = null;
            String groupUri = null;
            String permission = null;
            bool inGrant = false;
            StringBuilder curText = new StringBuilder();


            XmlReader xr = XmlReader.Create(new BufferedStream(UnmarshallerUtils.sanitizeXmlDocument(inputStream)));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.Element))
                {
                    if (xr.Name.Equals("Grant"))
                        inGrant = true;
                    else if (xr.Name.Equals("Grantee"))
                        granteeType = xr.GetAttribute("xsi:type");
                }
                else if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("DisplayName"))
                    {
                        if (!inGrant)
                            ownerId = curText.ToString();
                        else
                            userId = curText.ToString();
                    }
                    else if (xr.Name.Equals("ID"))
                    {
                        if (!inGrant)
                            ownerDisplayName = curText.ToString();
                        else
                            userDisplayName = curText.ToString();
                    }
                    else if (xr.Name.Equals("URI"))
                        groupUri = curText.ToString();
                    else if (xr.Name.Equals("Owner"))
                    {
                        owner = new Owner(ownerId, ownerDisplayName);
                        acl.setOwner(owner);
                    }
                    else if (xr.Name.Equals("Grantee"))
                    {
                        if (granteeType.Equals("CanonicalUser"))
                            grantee = new CanonicalGrantee(userId, userDisplayName);
                        else if (granteeType.Equals("Group"))
                            grantee = new GroupGrantee(groupUri);
                    }
                    else if (xr.Name.Equals("Permission"))
                        permission = curText.ToString();
                    else if (xr.Name.Equals("Grant"))
                    {
                        acl.grantPermission(grantee, permission);
                        inGrant = false;
                    }

                    curText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    curText.Append(xr.Value);
                }
            } // end while

            return acl;
        } // end of unmarshall
    }
}
