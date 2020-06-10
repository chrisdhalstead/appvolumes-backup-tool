Public Class AppVolumes

    Public Class storage

        Public Property id() As Integer
        Public Property name() As String
        Public Property host() As String
        Public Property space_used() As String
        Public Property space_total() As String
        Public Property num_appstacks() As String
        Public Property num_writables() As String
        Public Property attachable() As String
        Public Property created_at() As String
        Public Property created_at_human() As String

    End Class

    Public Class appstacks

        Public Property id() As Integer
        Public Property name() As String
        Public Property name_html() As String
        Public Property path() As String
        Public Property datastore_name() As String
        Public Property status() As String
        Public Property created_at() As String
        Public Property created_at_human() As String
        Public Property mounted_at() As String
        Public Property mounted_at_human() As String
        Public Property mount_count() As String
        Public Property size_mb() As String
        Public Property template_version() As String
        Public Property assignments_total() As String
        Public Property attachments_total() As String

    End Class

    Public Class Version

        Public Property version() As String
        Public Property internal() As String
        Public Property copyright() As String
        Public Property configured() As String

    End Class

    Public Class writables

        Public Property id() As Integer
        Public Property name() As String
        Public Property name_html() As String
        Public Property owner() As String
        Public Property link() As String
        Public Property owner_name() As String
        Public Property owner_type() As String
        Public Property created_at() As String
        Public Property created_at_human() As String
        Public Property mounted_at() As String
        Public Property mounted_at_human() As String
        Public Property attached() As String
        Public Property status() As String
        Public Property mount_count() As String
        Public Property size_mb() As String
        Public Property free_mb() As String
        Public Property total_mb() As String
        Public Property percent_available() As String
        Public Property template_version() As String
        Public Property datastore_name() As String

    End Class


    Public Class WritableData

        Public Property id() As Integer
        Public Property name() As String
        Public Property name_html() As String
        Public Property owner() As String
        Public Property link() As String
        Public Property owner_name() As String
        Public Property owner_type() As String
        Public Property description() As String
        Public Property created_at() As String
        Public Property created_at_human() As String
        Public Property mounted_at() As String
        Public Property mounted_at_human() As String
        Public Property mount_count() As String
        Public Property attached() As String
        Public Property version_tag() As String
        Public Property block_login() As String
        Public Property mount_prefix() As String
        Public Property defer_create() As String
        Public Property size_mb() As String
        Public Property template_version() As String
        Public Property datastore_name() As String
        Public Property machine_manager_host() As String
        Public Property machine_manager_type() As String
        Public Property path() As String
        Public Property filename() As String
        Public Property file_location() As String
        Public Property template_file_name() As String
        Public Property [protected]() As String
        Public Property free_mb() As String
        Public Property total_mb() As String
        Public Property percent_available() As String
        Public Property can_expand() As String
        Public Property primordial_os_id() As String
        Public Property primordial_os_name() As String
        Public Property oses() As ArrayList

    End Class


    Public Class file_locations

        Public Property name() As String
        Public Property path() As String
        Public Property missing() As String
        Public Property reachable() As String
        Public Property storage_location() As String
        Public Property machine_manager_host() As String
        Public Property machine_manager_type() As String
        Public Property created_at() As String
        Public Property created_at_human() As String

    End Class

    Public Class applications

        Public Property id() As Integer
        Public Property name() As String
        Public Property guid() As String
        Public Property icon() As String
        Public Property ing_icon() As String
        Public Property status() As String
        Public Property version() As String
        Public Property publisher() As String
        Public Property created_at() As String
        Public Property created_at_human() As String

    End Class


    Public Class AppStackData

        Public Property id() As Integer
        Public Property name() As String
        Public Property name_html() As String
        Public Property path() As String
        Public Property datastore_name() As String
        Public Property filename() As String
        Public Property file_location() As String
        Public Property description() As String
        Public Property status() As String
        Public Property created_at() As String
        Public Property created_at_human() As String
        Public Property mounted_at() As String
        Public Property mounted_at_human() As String
        Public Property size_mb() As String
        Public Property template_version() As String
        Public Property mount_count() As String
        Public Property assignments_total() As String
        Public Property attachments_total() As String
        Public Property location_count() As String
        Public Property application_count() As String
        Public Property application_icons() As ArrayList
        Public Property volume_guid() As String
        Public Property template_file_name() As String
        Public Property agent_version() As String
        Public Property capture_version() As String
        Public Property primordial_os_id() As Integer
        Public Property primordial_os_name() As String
        Public Property oses() As ArrayList
        Public Property provision_duration() As String

    End Class


End Class
