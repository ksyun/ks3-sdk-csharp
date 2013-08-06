﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Represents a grantee identified by their canonical KS3 ID.
     * The canonical KS3 ID can be thought of as
     * an KS3-internal ID specific to a user. For
     * example, Amazon can map a grantee identified by an email address to a
     * canonical ID.
     * <p>
     * Canonical grantees may have an associated display name, which is a
     * human-friendly name that KS3 has linked to the canonical ID (eg. the user's
     * login name).
     * </p>
     */
    public class CanonicalGrantee : Grantee
    {
        private String id = null;
        private String displayName = null;

        public CanonicalGrantee(String id)
        {
            this.id = id;
        }

        public CanonicalGrantee(String id, String displayName)
        {
            this.id = id;
            this.displayName = displayName;
        }

        public String getTypeIdentifier()
        {
            return "id";
        }

        /**
         * Sets the unique identifier for this grantee.
         */
        public void setIdentifier(String id)
        {
            this.id = id;
        }

        /**
         * Returns the unique identifier for this grantee.
         */ 
        public String getIdentifier()
        {
            return this.id;
        }

        /**
         * Sets the display name for this grantee.
         */ 
        public void setDisplayName(String displayName)
        {
            this.displayName = displayName;
        }

        /**
         * Returns the display name for this grantee.
         */ 
        public String getDisplayName()
        {
            return this.displayName;
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType().Equals(this.GetType()))
            {
                CanonicalGrantee other = (CanonicalGrantee)obj;
                return this.id.Equals(other.id);
            }

            return false;
        }

        public override string ToString()
        {
            return "CanonicalGrantee [id=" + this.id + ", displayName=" + this.displayName + "]";
        }
    }
}
