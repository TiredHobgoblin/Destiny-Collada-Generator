import bpy
import xml.etree.ElementTree as ET

bl_info = {
    "name": "Import Collada Custom Normals",
    "blender": (2, 80, 0),
    "category": "Import-Export",
}

def read_some_data(context, filepath):
    print("Importing normals...")
    tree = ET.parse(filepath)
    COLLADA = tree.getroot()
    
    library_geometries = COLLADA.find("./{http://www.collada.org/2005/11/COLLADASchema}library_geometries")
    library_controllers = COLLADA.find("./{http://www.collada.org/2005/11/COLLADASchema}library_controllers")
    library_visual_scenes = COLLADA.find("./{http://www.collada.org/2005/11/COLLADASchema}library_visual_scenes")
    visual_scene = library_visual_scenes.find("./{http://www.collada.org/2005/11/COLLADASchema}visual_scene")
    
    normals = {"objectname" : "objectnormals"}
    
    for node in visual_scene.iter("{http://www.collada.org/2005/11/COLLADASchema}node"):
        numberwithname = 0
        for object in bpy.context.selected_objects:
            if node.attrib["name"] == object.name:
                numberwithname += 1
        if numberwithname == 0:
            continue
        
        # Locate geometry for scene node
        instance_geometry = node.find("./{http://www.collada.org/2005/11/COLLADASchema}instance_geometry")
        meshid = ""
        if instance_geometry is None:
            instance_controller = node.find("./{http://www.collada.org/2005/11/COLLADASchema}instance_controller")
            if instance_controller is None:
                continue
            controllerid = instance_controller.attrib["url"].replace("#", "")
            controller = library_controllers.find("./{http://www.collada.org/2005/11/COLLADASchema}controller[@id='"+controllerid+"']")
            meshid = controller.find("./{http://www.collada.org/2005/11/COLLADASchema}skin").attrib["source"].replace("#", "")
        else:
            meshid = instance_geometry.attrib["url"].replace("#", "")
        geometry = library_geometries.find("./{http://www.collada.org/2005/11/COLLADASchema}geometry[@id='"+meshid+"']")
        
        # Pick out normals
        mesh = geometry.find("./{http://www.collada.org/2005/11/COLLADASchema}mesh")
        triangles = mesh.find("./{http://www.collada.org/2005/11/COLLADASchema}triangles")
        normalinput = triangles.find("./{http://www.collada.org/2005/11/COLLADASchema}input[@semantic='NORMAL']")
        if normalinput is None:
            print ( node.attrib["name"] + " has no custom normals.")
            continue
        normalid = normalinput.attrib["source"].replace("#", "")
        normalsource = mesh.find("./{http://www.collada.org/2005/11/COLLADASchema}source[@id='"+normalid+"']")
        normals[node.attrib["name"]] = normalsource.find("./{http://www.collada.org/2005/11/COLLADASchema}float_array").text.split()
        
        for object in bpy.context.selected_objects:
            if node.attrib["name"] != object.name:
                continue
            formattednormals = [[0,0,0] for i in range(len(normals[node.attrib["name"]])//3)]
            for i in range(len(normals[node.attrib["name"]])):
                formattednormals[i//3][i%3] = float(normals[node.attrib["name"]][i])
                                
            bpy.context.view_layer.objects.active = object
            bpy.ops.mesh.customdata_custom_splitnormals_add()
            object.data.use_auto_smooth = True
            
            object.data.normals_split_custom_set_from_vertices(formattednormals)
            
            print("Imported normals for "+object.name+".")
            
    print("Done.")

    return {'FINISHED'}


# ImportHelper is a helper class, defines filename and
# invoke() function which calls the file selector.
from bpy_extras.io_utils import ImportHelper
from bpy.props import StringProperty, BoolProperty, EnumProperty
from bpy.types import Operator


class ImportSomeData(Operator, ImportHelper):
    """Import custom mesh normals from a Collada file."""
    bl_idname = "import_test.some_data"  # important since its how bpy.ops.import_test.some_data is constructed
    bl_label = "Import Normals"

    # ImportHelper mixin class uses this
    filename_ext = ".dae"

    filter_glob: StringProperty(
        default="*.dae",
        options={'HIDDEN'},
        maxlen=255,  # Max internal buffer length, longer would be clamped.
    )
    
    def execute(self, context):
        return read_some_data(context, self.filepath)


# Only needed if you want to add into a dynamic menu
def menu_func_import(self, context):
    self.layout.operator(ImportSomeData.bl_idname, text="Import Collada Normals (.dae)")


def register():
    bpy.utils.register_class(ImportSomeData)
    bpy.types.TOPBAR_MT_file_import.append(menu_func_import)


def unregister():
    bpy.utils.unregister_class(ImportSomeData)
    bpy.types.TOPBAR_MT_file_import.remove(menu_func_import)


if __name__ == "__main__":
    register()

    # test call
    bpy.ops.import_test.some_data('INVOKE_DEFAULT')
