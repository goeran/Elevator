Elevator
========

A simple to use data migration tool:)

##Background
All software projects, at least the ones I have participated in, have at some point needed to handle migration of data. Most of the time we have done it manually, and other times we have created a tool or used a tool to do migration. There are many good tools out there today for migration, but I could not find anyone that satisfied my needs. Most of the tools out there today are geared towards databases, and I’m not always working against those databases – sometimes I store data in text files. There for I set out on this journey to create a super simple migration tool that can help me handle migration in projects.

##Solution
This tools his heavily inspired by the migration framework in Ruby active records. It stores the migration state in a safe place (database, file and so forth) and it handles the lifting when going from “level 1” to “level 2”. Each level can contain changes in data, data structure and security. Let’s say you have introduced a new property on the User object, and you need to update the database schema and the actual value for all the existing users. This value might be calculated based on existing data. This is a perfect use case for this migration tool.

The framework aims to be a generic framework where you can plug into and make it fit your needs. 

The migration tool is a console application that can be executed both manually and by a build server. 

