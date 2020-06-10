# appvolumes-backup-tool
Utility to Backup AppVolumes
Chris Halstead
@chrisdhalstead

This utility is provided as-is and there is no support

Instructions:

Make sure the Microsoft .NET Framework and VMware PowerCLI are installed on the system that you will run the backup utility from.
Launch the utility and type the HTTP URL into your App Volumes manager (Example: https://av-manager.company.com). You will be prompted to enter a username and password. The username should be a member of the App Volumes Administrators group. Note: The user MUST be in the App Volumes Administrators group in order to access the App Volumes API.
Click "Populate AppStacks". This will list all AppStacks and Writable Volumes on that App Volumes Manager. Note: You can right-click an AppStack and choose "Show Installed Applications" to see what applications are contained within that AppStack.
Enter the URL for the Virtual Center that you would like to use for backing up App Volumes data. Example: https://vcenter.company.com.  Click "Populate VC Data" to connect and enumerate the vSphere hosts located on this vCenter. Note: The user will need rights to browse, add/remove Hard Disks, and Create and Delete Virtual Machines in vCenter.
Create a Backup VM: Click "Create a Backup VM". You will be prompted to select a vSphere host and a Datastore on that host to place the Backup VM. The backup VM is a very small VM which will be used to attach AppStacks and Writable Volumes. The VM is never powered on-- it is simply used as a backup proxy.
Once the Backup VM is created, you can choose which AppStacks and Writable Volumes you would like to back up, and attach them to the Backup VM.
Select the Backup VM you would like to use by clicking the check box next to the name of the VM.
Select each AppStack and Writable Volume that you would like to back up by selecting the check box next to the name of the AppStack or Writable Volume.
Click "Attach Selected AppStacks to Backup VM". This will attach the underlying VMDK files for each of the AppStacks and Writable Volumes to the Backup VM. You will see that each of the AppStacks and Writable Volumes you selected is now attached to the Backup VM.
If you would like to remove any of the AppStacks or Writable Volumes that are attached to the Backup VM, simply select them by checking next to the name and selecting "Detach Selected AppStacks from Backup VM".
At this time, you can use a backup solution which is VMDK-aware to back up the AppStacks and Writable Volumes.
Note: Multiple Backup VMs can be created and used.

Configuring and Using the Backup Prep Utility:

This utility is used in conjunction with a backup to make sure that Writable Volumes are not user-attached. If a Writable Volume is user-attached when it is attempted to be backed up, the snapshot process (and therefore the backup of the Writable Volume) will fail. This utility looks at all Writable Volumes connected to the Backup VM. It checks to see if they are user-attached at the time of back up. If the writable is user-attached, it is removed from the Backup VM and the name is written to a temporary XML file. Other Writable Volumes which are not user-attached are disabled for the duration of the back up. After the back up, the utility will review the XML file (if created) to see what Writable Volumes need to be re-attached to the Backup VM. It will re-attach the Writable Volumes that were user-attached, and re-enable all of the Writable Volumes.

Usage: There are pre-backup.bat and post.backup.bat files located in the Backup Prep folder as part of this application download.

1. Place the Backup Prep Folder on the Backup Server

2. Update the pre-backup.bat and post-backup.bat files with information on your environment. Example:

Pre-Backup: av_backup_prep.exe https://avmanager domain\user Password vCenter_Address Backup_VM_Name PRE
Post-Backup: av_backup_prep.exe http://avmanager domain\user Password vCenter_Address Backup_VM_Name POST
3. Call the batch files from the Backup Job to run before and after the Backup Job runs. (This utility was tested with Veeam Backup & Replication 8.0).

For more information on instructions, please see this article.

Note: This tool backs up the vmdk portion of the App Volumes AppStack. Ideally, whenever possible, it is recommended to separately backup the metadata file of those AppStacks. After the restore operation, you should copy back the metadata file in the same location as the AppStacks. Restoring without the metadata information will still work but it will mark your AppStacks as Legacy. In the future, we plan to work on a version of the tool that will support adding the Metadata information as well.
