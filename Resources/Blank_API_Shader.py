import bpy


class NODE_PT_MAINPANEL(bpy.types.Panel):
    bl_label = "Custom Node Group"
    bl_idname = "NODE_PT_MAINPANEL"
    bl_space_type = 'NODE_EDITOR'
    bl_region_type = 'UI'
    bl_category = 'D2 Shader' 

    def draw(self, context):
        layout = self.layout

        row = layout.row()
        row.operator('node.test_operator')





def create_test_group(context, operator, group_name):
    
        #enable use nodes
    bpy.context.scene.use_nodes = True
    
    test_group = bpy.data.node_groups.new(group_name, 'ShaderNodeTree')

##Activate Node Connection usage
    link = test_group.links.new

##Nodegroup Outputs##
    group_out = test_group.nodes.new('NodeGroupOutput')
    group_out.location = (1500,-1000)
    test_group.outputs.new('NodeSocketColor','Dye Color A')
    test_group.outputs.new('NodeSocketColor','Dye Color B')
    test_group.outputs.new('NodeSocketColor','Glossiness Vector A')
    test_group.outputs.new('NodeSocketColor','Glossiness Vector B')
    test_group.outputs.new('NodeSocketColor','Glossiness Vector C')
    test_group.outputs.new('NodeSocketColor','Wear Vector A')
    test_group.outputs.new('NodeSocketColor','Wear Vector B')
    test_group.outputs.new('NodeSocketColor','TiledNormal/TiledNormalStrength')
    test_group.outputs.new('NodeSocketColor','Metalness/Specular/Iridescence A')
    test_group.outputs.new('NodeSocketColor','Metalness/Specular/Iridescence B')
    test_group.outputs.new('NodeSocketColor','Emission')

#--------------------------------------------------------------------------------------------
##Vertex Color Node##
    VertexColor = test_group.nodes.new(type= 'ShaderNodeAttribute')
    VertexColor.attribute_name = ("slots")
    VertexColor.hide = (1)

##Vertex Color SeparateRGB Node##
    VertexColorRGBSplit = test_group.nodes.new(type= 'ShaderNodeSeparateRGB')
    VertexColorRGBSplit.location = (170,0)
    VertexColorRGBSplit.hide = (1)

##VertexPaint ArmorPrimary Separator##
    Armor_Primary_Slot = test_group.nodes.new(type= 'ShaderNodeMath')
    Armor_Primary_Slot.label = "Armor_Primary_Slot"
    Armor_Primary_Slot.operation = 'LESS_THAN'
    Armor_Primary_Slot.inputs[1].default_value = (0.333)
    Armor_Primary_Slot.location = (350,100)
    Armor_Primary_Slot.hide = (1)

#There is no need for the Armor Secondary Separator so it isn't here

##VertexPaint ClothPrimary Separator##
    Cloth_Primary_Slot = test_group.nodes.new(type= 'ShaderNodeMath')
    Cloth_Primary_Slot.label = "Cloth_Primary_Slot"
    Cloth_Primary_Slot.operation = 'GREATER_THAN'
    Cloth_Primary_Slot.inputs[1].default_value = (0.999)
    Cloth_Primary_Slot.location = (350,70)
    Cloth_Primary_Slot.hide = (1)

##VertexPaint ClothSecondary Separator##
    Cloth_Secondary_Slot_LESSTHAN = test_group.nodes.new(type= 'ShaderNodeMath')
    Cloth_Secondary_Slot_LESSTHAN.label = " "
    Cloth_Secondary_Slot_LESSTHAN.operation = 'LESS_THAN'
    Cloth_Secondary_Slot_LESSTHAN.inputs[1].default_value = (0.333)
    Cloth_Secondary_Slot_LESSTHAN.location = (350,40)
    Cloth_Secondary_Slot_LESSTHAN.hide = (1)

    Cloth_Secondary_Slot_GREATERTHAN = test_group.nodes.new(type= 'ShaderNodeMath')
    Cloth_Secondary_Slot_GREATERTHAN.label = " "
    Cloth_Secondary_Slot_GREATERTHAN.operation = 'GREATER_THAN'
    Cloth_Secondary_Slot_GREATERTHAN.inputs[1].default_value = (0.000)
    Cloth_Secondary_Slot_GREATERTHAN.location = (350,10)
    Cloth_Secondary_Slot_GREATERTHAN.hide = (1)
    
    Cloth_Secondary_Slot = test_group.nodes.new(type= 'ShaderNodeMath')
    Cloth_Secondary_Slot.label = "Cloth_Secondary_Slot"
    Cloth_Secondary_Slot.operation = 'MULTIPLY'
    Cloth_Secondary_Slot.location = (520,25)
    Cloth_Secondary_Slot.hide = (1)

##VertexPaint SuitPrimary Separator##
    Suit_Primary_Slot_LESSTHAN = test_group.nodes.new(type= 'ShaderNodeMath')
    Suit_Primary_Slot_LESSTHAN.label = " "
    Suit_Primary_Slot_LESSTHAN.operation = 'LESS_THAN'
    Suit_Primary_Slot_LESSTHAN.inputs[1].default_value = (0.666)
    Suit_Primary_Slot_LESSTHAN.location = (350,-20)
    Suit_Primary_Slot_LESSTHAN.hide = (1)

    Suit_Primary_Slot_GREATERTHAN = test_group.nodes.new(type= 'ShaderNodeMath')
    Suit_Primary_Slot_GREATERTHAN.label = " "
    Suit_Primary_Slot_GREATERTHAN.operation = 'GREATER_THAN'
    Suit_Primary_Slot_GREATERTHAN.inputs[1].default_value = (0.333)
    Suit_Primary_Slot_GREATERTHAN.location = (350,-50)
    Suit_Primary_Slot_GREATERTHAN.hide = (1)

    Suit_Primary_Slot = test_group.nodes.new(type= 'ShaderNodeMath')
    Suit_Primary_Slot.label = "Suit_Primary_Slot"
    Suit_Primary_Slot.operation = 'MULTIPLY'
    Suit_Primary_Slot.location = (520,-35)
    Suit_Primary_Slot.hide = (1)

##VertexPaint SuitSecondary Separator##
    Suit_Secondary_Slot = test_group.nodes.new(type= 'ShaderNodeMath')
    Suit_Secondary_Slot.label = "Suit_Secondary_Slot"
    Suit_Secondary_Slot.operation = 'GREATER_THAN'
    Suit_Secondary_Slot.inputs[1].default_value = (0.999)
    Suit_Secondary_Slot.location = (350,-80)
    Suit_Secondary_Slot.hide = (1)

#Connect Nodes for Slot Isolation nodes
    link(VertexColor.outputs[0], VertexColorRGBSplit.inputs[0])

    link(VertexColorRGBSplit.outputs[0], Armor_Primary_Slot.inputs[0])

    link(VertexColorRGBSplit.outputs[0], Cloth_Primary_Slot.inputs[0])

    link(VertexColorRGBSplit.outputs[1], Cloth_Secondary_Slot_LESSTHAN.inputs[0])
    link(VertexColorRGBSplit.outputs[1], Cloth_Secondary_Slot_GREATERTHAN.inputs[0])
    link(Cloth_Secondary_Slot_LESSTHAN.outputs[0], Cloth_Secondary_Slot.inputs[0])
    link(Cloth_Secondary_Slot_GREATERTHAN.outputs[0], Cloth_Secondary_Slot.inputs[1])

    link(VertexColorRGBSplit.outputs[1], Suit_Primary_Slot_LESSTHAN.inputs[0])
    link(VertexColorRGBSplit.outputs[1], Suit_Primary_Slot_GREATERTHAN.inputs[0])
    link(Suit_Primary_Slot_LESSTHAN.outputs[0], Suit_Primary_Slot.inputs[0])
    link(Suit_Primary_Slot_GREATERTHAN.outputs[0], Suit_Primary_Slot.inputs[1])

    link(Suit_Primary_Slot_GREATERTHAN.outputs[0], Suit_Primary_Slot.inputs[1])

    link(VertexColorRGBSplit.outputs[1], Suit_Secondary_Slot.inputs[0])
#--------------------------------------------------------------------------------------------

#MixNode Cluster DyeColor A
    DyeColor1MixNodeA = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    DyeColor1MixNodeA.label = "DyeColorA"
    DyeColor1MixNodeA.inputs[2].default_value = (1.000, 0.000, 0.000, 1) #BIOS ArmorPrimary Color [X,X,X,X]
    DyeColor1MixNodeA.inputs[1].default_value = (1.000, 1.000, 0.000, 1) #BIOS ArmorSecondary Color [X,X,X,X]
    DyeColor1MixNodeA.location = (500,-220)
    DyeColor1MixNodeA.hide = (1)

    DyeColor1MixNodeB = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    DyeColor1MixNodeB.label = "DyeColorA"
    DyeColor1MixNodeB.inputs[2].default_value = (0.000, 1.000, 0.000, 1) #BIOS ClothPrimary Color [X,X,X,X]
    DyeColor1MixNodeB.location = (500,-250)
    DyeColor1MixNodeB.hide = (1)

    DyeColor1MixNodeC = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    DyeColor1MixNodeC.label = "DyeColorA"
    DyeColor1MixNodeC.inputs[2].default_value = (0.000, 1.000, 1.000, 1) #BIOS ClothSecondary Color [X,X,X,X]
    DyeColor1MixNodeC.location = (500,-280)
    DyeColor1MixNodeC.hide = (1)

    DyeColor1MixNodeD = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    DyeColor1MixNodeD.label = "DyeColorA"
    DyeColor1MixNodeD.inputs[2].default_value = (0.000, 0.000, 1.000, 1) #BIOS SuitPrimary Color [X,X,X,X]
    DyeColor1MixNodeD.location = (500,-310)
    DyeColor1MixNodeD.hide = (1)

    DyeColor1MixNodeE = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    DyeColor1MixNodeE.label = "DyeColorA"
    DyeColor1MixNodeE.inputs[2].default_value = (1.000, 1.000, 1.000, 1) #BIOS SuitSecondary Color [X,X,X,X]
    DyeColor1MixNodeE.location = (500,-340)
    DyeColor1MixNodeE.hide = (1)

    link(DyeColor1MixNodeA.outputs[0], DyeColor1MixNodeB.inputs[1])
    link(DyeColor1MixNodeB.outputs[0], DyeColor1MixNodeC.inputs[1])
    link(DyeColor1MixNodeC.outputs[0], DyeColor1MixNodeD.inputs[1])
    link(DyeColor1MixNodeD.outputs[0], DyeColor1MixNodeE.inputs[1])
    link(DyeColor1MixNodeE.outputs[0], group_out.inputs[0])

    link(Armor_Primary_Slot.outputs[0], DyeColor1MixNodeA.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], DyeColor1MixNodeB.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], DyeColor1MixNodeC.inputs[0])
    link(Suit_Primary_Slot.outputs[0], DyeColor1MixNodeD.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], DyeColor1MixNodeE.inputs[0])


#MixNode Cluster DyeColor B
    DyeColor2MixNodeA = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    DyeColor2MixNodeA.label = "DyeColorB"
    DyeColor2MixNodeA.inputs[2].default_value = (1.000, 0.000, 0.000, 1) #BIOS ArmorPrimary Wear Color [X,X,X,X]
    DyeColor2MixNodeA.inputs[1].default_value = (1.000, 1.000, 0.000, 1) #BIOS ArmorSecondary Wear Color [X,X,X,X]
    DyeColor2MixNodeA.location = (500,-400)
    DyeColor2MixNodeA.hide = (1)

    DyeColor2MixNodeB = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    DyeColor2MixNodeB.label = "DyeColorB"
    DyeColor2MixNodeB.inputs[2].default_value = (0.000, 1.000, 0.000, 1) #BIOS ClothPrimary Wear Color [X,X,X,X]
    DyeColor2MixNodeB.location = (500,-430)
    DyeColor2MixNodeB.hide = (1)

    DyeColor2MixNodeC = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    DyeColor2MixNodeC.label = "DyeColorB"
    DyeColor2MixNodeC.inputs[2].default_value = (0.000, 1.000, 1.000, 1) #BIOS ClothSecondary Wear Color [X,X,X,X]
    DyeColor2MixNodeC.location = (500,-460)
    DyeColor2MixNodeC.hide = (1)

    DyeColor2MixNodeD = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    DyeColor2MixNodeD.label = "DyeColorB"
    DyeColor2MixNodeD.inputs[2].default_value = (0.000, 0.000, 1.000, 1) #BIOS SuitPrimary Wear Color [X,X,X,X]
    DyeColor2MixNodeD.location = (500,-490)
    DyeColor2MixNodeD.hide = (1)

    DyeColor2MixNodeE = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    DyeColor2MixNodeE.label = "DyeColorB"
    DyeColor2MixNodeE.inputs[2].default_value = (1.000, 1.000, 1.000, 1) #BIOS SuitSecondary Wear Color [X,X,X,X]
    DyeColor2MixNodeE.location = (500,-520)
    DyeColor2MixNodeE.hide = (1)

    link(DyeColor2MixNodeA.outputs[0], DyeColor2MixNodeB.inputs[1])
    link(DyeColor2MixNodeB.outputs[0], DyeColor2MixNodeC.inputs[1])
    link(DyeColor2MixNodeC.outputs[0], DyeColor2MixNodeD.inputs[1])
    link(DyeColor2MixNodeD.outputs[0], DyeColor2MixNodeE.inputs[1])
    link(DyeColor2MixNodeE.outputs[0], group_out.inputs[1])

    link(Armor_Primary_Slot.outputs[0], DyeColor2MixNodeA.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], DyeColor2MixNodeB.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], DyeColor2MixNodeC.inputs[0])
    link(Suit_Primary_Slot.outputs[0], DyeColor2MixNodeD.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], DyeColor2MixNodeE.inputs[0])

#MixNode Cluster Wear Remap Vector A Paramaters
    WearRemapA = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    WearRemapA.label = "Wear Remap"
    WearRemapA.location = (500,-580)
    WearRemapA.hide = (1)

    WearRemapB = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    WearRemapB.label = "Wear Remap"
    WearRemapB.location = (500,-610)
    WearRemapB.hide = (1)

    WearRemapC = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    WearRemapC.label = "Wear Remap"
    WearRemapC.location = (500,-640)
    WearRemapC.hide = (1)

    WearRemapD = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    WearRemapD.label = "Wear Remap"
    WearRemapD.location = (500,-670)
    WearRemapD.hide = (1)

    WearRemapE = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    WearRemapE.label = "Wear Remap"
    WearRemapE.location = (500,-700)
    WearRemapE.hide = (1)

    link(WearRemapA.outputs[0], WearRemapB.inputs[1])
    link(WearRemapB.outputs[0], WearRemapC.inputs[1])
    link(WearRemapC.outputs[0], WearRemapD.inputs[1])
    link(WearRemapD.outputs[0], WearRemapE.inputs[1])
    link(WearRemapE.outputs[0], group_out.inputs[5])

    link(Armor_Primary_Slot.outputs[0], WearRemapA.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], WearRemapB.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], WearRemapC.inputs[0])
    link(Suit_Primary_Slot.outputs[0], WearRemapD.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], WearRemapE.inputs[0])

#MixNode Cluster Wear Remap Vector B Paramaters
    WearRemapF = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    WearRemapF.label = "Wear Remap"
    WearRemapF.location = (500,-760)
    WearRemapF.hide = (1)

    WearRemapG = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    WearRemapG.label = "Wear Remap"
    WearRemapG.location = (500,-790)
    WearRemapG.hide = (1)

    WearRemapH = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    WearRemapH.label = "Wear Remap"
    WearRemapH.location = (500,-820)
    WearRemapH.hide = (1)

    WearRemapI = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    WearRemapI.label = "Wear Remap"
    WearRemapI.location = (500,-850)
    WearRemapI.hide = (1)

    WearRemapJ = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    WearRemapJ.label = "Wear Remap"
    WearRemapJ.location = (500,-880)
    WearRemapJ.hide = (1)

    link(WearRemapF.outputs[0], WearRemapG.inputs[1])
    link(WearRemapG.outputs[0], WearRemapH.inputs[1])
    link(WearRemapH.outputs[0], WearRemapI.inputs[1])
    link(WearRemapI.outputs[0], WearRemapJ.inputs[1])
    link(WearRemapJ.outputs[0], group_out.inputs[6])

    link(Armor_Primary_Slot.outputs[0], WearRemapF.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], WearRemapG.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], WearRemapH.inputs[0])
    link(Suit_Primary_Slot.outputs[0], WearRemapI.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], WearRemapJ.inputs[0])

#MixNode Cluster Glossiness Remap Vector A Paramaters
    GlossA = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossA.label = "GlossRemap_A"
    GlossA.location = (500,-940)
    GlossA.hide = (1)

    GlossB = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossB.label = "GlossRemap_A"
    GlossB.location = (500,-970)
    GlossB.hide = (1)

    GlossC = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossC.label = "GlossRemap_A"
    GlossC.location = (500,-1000)
    GlossC.hide = (1)

    GlossD = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossD.label = "GlossRemap_A"
    GlossD.location = (500,-1030)
    GlossD.hide = (1)

    GlossE = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossE.label = "GlossRemap_A"
    GlossE.location = (500,-1060)
    GlossE.hide = (1)

    link(GlossA.outputs[0], GlossB.inputs[1])
    link(GlossB.outputs[0], GlossC.inputs[1])
    link(GlossC.outputs[0], GlossD.inputs[1])
    link(GlossD.outputs[0], GlossE.inputs[1])
    link(GlossE.outputs[0], group_out.inputs[2])

    link(Armor_Primary_Slot.outputs[0], GlossA.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], GlossB.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], GlossC.inputs[0])
    link(Suit_Primary_Slot.outputs[0], GlossD.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], GlossE.inputs[0])

#MixNode Cluster Glossiness Remap Vector B Paramaters
    GlossF = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossF.label = "GlossRemap_B"
    GlossF.location = (500,-1120)
    GlossF.hide = (1)

    GlossG = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossG.label = "GlossRemap_B"
    GlossG.location = (500,-1150)
    GlossG.hide = (1)

    GlossH = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossH.label = "GlossRemap_B"
    GlossH.location = (500,-1180)
    GlossH.hide = (1)

    GlossI = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossI.label = "GlossRemap_B"
    GlossI.location = (500,-1210)
    GlossI.hide = (1)

    GlossJ = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossJ.label = "GlossRemap_B"
    GlossJ.location = (500,-1240)
    GlossJ.hide = (1)

    link(GlossF.outputs[0], GlossG.inputs[1])
    link(GlossG.outputs[0], GlossH.inputs[1])
    link(GlossH.outputs[0], GlossI.inputs[1])
    link(GlossI.outputs[0], GlossJ.inputs[1])
    link(GlossJ.outputs[0], group_out.inputs[3])

    link(Armor_Primary_Slot.outputs[0], GlossF.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], GlossG.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], GlossH.inputs[0])
    link(Suit_Primary_Slot.outputs[0], GlossI.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], GlossJ.inputs[0])

#MixNode Cluster Glossiness Remap Vector C Paramaters
    GlossK = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossK.label = "GlossRemap_C"
    GlossK.location = (500,-1300)
    GlossK.hide = (1)

    GlossL = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossL.label = "GlossRemap_C"
    GlossL.location = (500,-1330)
    GlossL.hide = (1)

    GlossM = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossM.label = "GlossRemap_C"
    GlossM.location = (500,-1360)
    GlossM.hide = (1)

    GlossN = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossN.label = "GlossRemap_C"
    GlossN.location = (500,-1390)
    GlossN.hide = (1)

    GlossO = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    GlossO.label = "GlossRemap_C"
    GlossO.location = (500,-1420)
    GlossO.hide = (1)

    link(GlossK.outputs[0], GlossL.inputs[1])
    link(GlossL.outputs[0], GlossM.inputs[1])
    link(GlossM.outputs[0], GlossN.inputs[1])
    link(GlossN.outputs[0], GlossO.inputs[1])
    link(GlossO.outputs[0], group_out.inputs[4])

    link(Armor_Primary_Slot.outputs[0], GlossK.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], GlossL.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], GlossM.inputs[0])
    link(Suit_Primary_Slot.outputs[0], GlossN.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], GlossO.inputs[0])

#MixNode Cluster Detail Noraml map
    TiledNormal_A = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    TiledNormal_A.label = "TiledNormal"
    TiledNormal_A.location = (500,-1480)
    TiledNormal_A.hide = (1)

    TiledNormal_B = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    TiledNormal_B.label = "TiledNormal"
    TiledNormal_B.location = (500,-1510)
    TiledNormal_B.hide = (1)

    TiledNormal_C = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    TiledNormal_C.label = "TiledNormal"
    TiledNormal_C.location = (500,-1540)
    TiledNormal_C.hide = (1)

    TiledNormal_D = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    TiledNormal_D.label = "TiledNormal"
    TiledNormal_D.location = (500,-1570)
    TiledNormal_D.hide = (1)

    TiledNormal_E = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    TiledNormal_E.label = "TiledNormal"
    TiledNormal_E.location = (500,-1600)
    TiledNormal_E.hide = (1)

    link(TiledNormal_A.outputs[0], TiledNormal_B.inputs[1])
    link(TiledNormal_B.outputs[0], TiledNormal_C.inputs[1])
    link(TiledNormal_C.outputs[0], TiledNormal_D.inputs[1])
    link(TiledNormal_D.outputs[0], TiledNormal_E.inputs[1])
    link(TiledNormal_E.outputs[0], group_out.inputs[7])

    link(Armor_Primary_Slot.outputs[0], TiledNormal_A.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], TiledNormal_B.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], TiledNormal_C.inputs[0])
    link(Suit_Primary_Slot.outputs[0], TiledNormal_D.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], TiledNormal_E.inputs[0])

#MixNode Cluster Metal/Specular/Iridescence Parameters
    MSI_A = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    MSI_A.label = "Metal/Specular/Iridescence"
    MSI_A.location = (500,-1660)
    MSI_A.hide = (1)

    MSI_B = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    MSI_B.label = "Metal/Specular/Iridescence"
    MSI_B.location = (500,-1690)
    MSI_B.hide = (1)

    MSI_C = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    MSI_C.label = "Metal/Specular/Iridescence"
    MSI_C.location = (500,-1720)
    MSI_C.hide = (1)

    MSI_D = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    MSI_D.label = "Metal/Specular/Iridescence"
    MSI_D.location = (500,-1750)
    MSI_D.hide = (1)

    MSI_E = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    MSI_E.label = "Metal/Specular/Iridescence"
    MSI_E.location = (500,-1780)
    MSI_E.hide = (1)

    link(MSI_A.outputs[0], MSI_B.inputs[1])
    link(MSI_B.outputs[0], MSI_C.inputs[1])
    link(MSI_C.outputs[0], MSI_D.inputs[1])
    link(MSI_D.outputs[0], MSI_E.inputs[1])
    link(MSI_E.outputs[0], group_out.inputs[8])

    link(Armor_Primary_Slot.outputs[0], MSI_A.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], MSI_B.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], MSI_C.inputs[0])
    link(Suit_Primary_Slot.outputs[0], MSI_D.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], MSI_E.inputs[0])

#MixNode Cluster Metal/Specular/Iridescence Parameters
    MSI_Worn_A = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    MSI_Worn_A.label = "Metal/Specular/Iridescence"
    MSI_Worn_A.location = (500,-1840)
    MSI_Worn_A.hide = (1)

    MSI_Worn_B = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    MSI_Worn_B.label = "Metal/Specular/Iridescence"
    MSI_Worn_B.location = (500,-1870)
    MSI_Worn_B.hide = (1)

    MSI_Worn_C = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    MSI_Worn_C.label = "Metal/Specular/Iridescence"
    MSI_Worn_C.location = (500,-1900)
    MSI_Worn_C.hide = (1)

    MSI_Worn_D = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    MSI_Worn_D.label = "Metal/Specular/Iridescence"
    MSI_Worn_D.location = (500,-1930)
    MSI_Worn_D.hide = (1)

    MSI_Worn_E = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    MSI_Worn_E.label = "Metal/Specular/Iridescence"
    MSI_Worn_E.location = (500,-1960)
    MSI_Worn_E.hide = (1)

    link(MSI_Worn_A.outputs[0], MSI_Worn_B.inputs[1])
    link(MSI_Worn_B.outputs[0], MSI_Worn_C.inputs[1])
    link(MSI_Worn_C.outputs[0], MSI_Worn_D.inputs[1])
    link(MSI_Worn_D.outputs[0], MSI_Worn_E.inputs[1])
    link(MSI_Worn_E.outputs[0], group_out.inputs[9])

    link(Armor_Primary_Slot.outputs[0], MSI_Worn_A.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], MSI_Worn_B.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], MSI_Worn_C.inputs[0])
    link(Suit_Primary_Slot.outputs[0], MSI_Worn_D.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], MSI_Worn_E.inputs[0])

#MixNode Cluster Emission Parameters
    Emission_A = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    Emission_A.inputs[2].default_value = (1.000, 1.000, 1.000, 1) #BIOS ArmorPrimary Emissive Tint Color and Intensity Bias [X,X,X,X]
    Emission_A.inputs[1].default_value = (1.000, 1.000, 1.000, 1) #BIOS ArmorSecondary Emissive Tint Color and Intensity Bias [X,X,X,X]
    Emission_A.label = "Emission"
    Emission_A.location = (500,-2020)
    Emission_A.hide = (1)

    Emission_B = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    Emission_B.inputs[2].default_value = (1.000, 1.000, 1.000, 1) #BIOS ClothPrimary Emissive Tint Color and Intensity Bias [X,X,X,X]
    Emission_B.label = "Emission"
    Emission_B.location = (500,-2050)
    Emission_B.hide = (1)

    Emission_C = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    Emission_C.inputs[2].default_value = (1.000, 1.000, 1.000, 1) #BIOS ClothSecondary Emissive Tint Color and Intensity Bias [X,X,X,X]
    Emission_C.label = "Emission"
    Emission_C.location = (500,-2080)
    Emission_C.hide = (1)

    Emission_D = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    Emission_D.inputs[2].default_value = (1.000, 1.000, 1.000, 1) #BIOS SuitPrimary Emissive Tint Color and Intensity Bias [X,X,X,X]
    Emission_D.label = "Emission"
    Emission_D.location = (500,-2110)
    Emission_D.hide = (1)

    Emission_E = test_group.nodes.new(type= 'ShaderNodeMixRGB')
    Emission_E.inputs[2].default_value = (1.000, 1.000, 1.000, 1) #BIOS SuitSecondary Emissive Tint Color and Intensity Bias [X,X,X,X]
    Emission_E.label = "Emission"
    Emission_E.location = (500,-2140)
    Emission_E.hide = (1)

    link(Emission_A.outputs[0], Emission_B.inputs[1])
    link(Emission_B.outputs[0], Emission_C.inputs[1])
    link(Emission_C.outputs[0], Emission_D.inputs[1])
    link(Emission_D.outputs[0], Emission_E.inputs[1])
    link(Emission_E.outputs[0], group_out.inputs[10])

    link(Armor_Primary_Slot.outputs[0], Emission_A.inputs[0])
    link(Cloth_Primary_Slot.outputs[0], Emission_B.inputs[0])
    link(Cloth_Secondary_Slot.outputs[0], Emission_C.inputs[0])
    link(Suit_Primary_Slot.outputs[0], Emission_D.inputs[0])
    link(Suit_Secondary_Slot.outputs[0], Emission_E.inputs[0])

#--------------------------------------------------------------------------------------------

#Wear Remap XYZ Nodes
    Wear_Remap01 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap01.label = "WearRemap"
    Wear_Remap01.inputs[0].default_value = (0.000) #BIOS ArmorPrimary Wear Remap: [X,_,_,_]
    Wear_Remap01.inputs[1].default_value = (0.000) #BIOS ArmorPrimary Wear Remap: [_,X,_,_]
    Wear_Remap01.inputs[2].default_value = (0.000) #BIOS ArmorPrimary Wear Remap: [_,_,X,_]
    Wear_Remap01.location = (0,-300)
    Wear_Remap01.hide = (0)

    Wear_Remap02 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap02.label = "WearRemap"
    Wear_Remap02.inputs[0].default_value = (0.000) #BIOS ArmorPrimary Wear Remap: [_,_,_,X]
    Wear_Remap02.inputs[1].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap02.inputs[2].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap02.location = (0,-420)
    Wear_Remap02.hide = (0)

    Wear_Remap03 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap03.label = "WearRemap"
    Wear_Remap03.inputs[0].default_value = (0.000) #BIOS ArmorSecondary Wear Remap: [X,_,_,_]
    Wear_Remap03.inputs[1].default_value = (0.000) #BIOS ArmorSecondary Wear Remap: [_,X,_,_]
    Wear_Remap03.inputs[2].default_value = (0.000) #BIOS ArmorSecondary Wear Remap: [_,_,X,_]
    Wear_Remap03.location = (0,-540)
    Wear_Remap03.hide = (0)

    Wear_Remap04 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap04.label = "WearRemap"
    Wear_Remap04.inputs[0].default_value = (0.000) #BIOS ArmorSecondary Wear Remap: [_,_,_,X]
    Wear_Remap04.inputs[1].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap04.inputs[2].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap04.location = (0,-660)
    Wear_Remap04.hide = (0)

    Wear_Remap05 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap05.label = "WearRemap"
    Wear_Remap05.inputs[0].default_value = (0.000) #BIOS ClothPrimary Wear Remap: [X,_,_,_]
    Wear_Remap05.inputs[1].default_value = (0.000) #BIOS ClothPrimary Wear Remap: [_,X,_,_]
    Wear_Remap05.inputs[2].default_value = (0.000) #BIOS ClothPrimary Wear Remap: [_,_,X,_]
    Wear_Remap05.location = (0,-780)
    Wear_Remap05.hide = (0)

    Wear_Remap06 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap06.label = "WearRemap"
    Wear_Remap06.inputs[0].default_value = (0.000) #BIOS ClothPrimary Wear Remap: [_,_,_,X]
    Wear_Remap06.inputs[1].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap06.inputs[2].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap06.location = (0,-900)
    Wear_Remap06.hide = (0)

    Wear_Remap07 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap07.label = "WearRemap"
    Wear_Remap07.inputs[0].default_value = (0.000) #BIOS ClothSecondary Wear Remap: [X,_,_,_]
    Wear_Remap07.inputs[1].default_value = (0.000) #BIOS ClothSecondary Wear Remap: [_,X,_,_]
    Wear_Remap07.inputs[2].default_value = (0.000) #BIOS ClothSecondary Wear Remap: [_,_,X,_]
    Wear_Remap07.location = (0,-1020)
    Wear_Remap07.hide = (0)

    Wear_Remap08 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap08.label = "WearRemap"
    Wear_Remap08.inputs[0].default_value = (0.000) #BIOS ClothSecondary Wear Remap: [_,_,_,X]
    Wear_Remap08.inputs[1].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap08.inputs[2].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap08.location = (0,-1140)
    Wear_Remap08.hide = (0)

    Wear_Remap09 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap09.label = "WearRemap"
    Wear_Remap09.inputs[0].default_value = (0.000) #BIOS SuitPrimary Wear Remap: [X,_,_,_]
    Wear_Remap09.inputs[1].default_value = (0.000) #BIOS SuitPrimary Wear Remap: [_,X,_,_]
    Wear_Remap09.inputs[2].default_value = (0.000) #BIOS SuitPrimary Wear Remap: [_,_,X,_]
    Wear_Remap09.location = (0,-1260)
    Wear_Remap09.hide = (0)

    Wear_Remap10 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap10.label = "WearRemap"
    Wear_Remap10.inputs[0].default_value = (0.000) #BIOS SuitPrimary Wear Remap: [_,_,_,X]
    Wear_Remap10.inputs[1].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap10.inputs[2].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap10.location = (0,-1380)
    Wear_Remap10.hide = (0)

    Wear_Remap11 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap11.label = "WearRemap"
    Wear_Remap11.inputs[0].default_value = (0.000) #BIOS SuitSecondary Wear Remap: [X,_,_,_]
    Wear_Remap11.inputs[1].default_value = (0.000) #BIOS SuitSecondary Wear Remap: [_,X,_,_]
    Wear_Remap11.inputs[2].default_value = (0.000) #BIOS SuitSecondary Wear Remap: [_,_,X,_]
    Wear_Remap11.location = (0,-1500)
    Wear_Remap11.hide = (0)

    Wear_Remap12 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Wear_Remap12.label = "WearRemap"
    Wear_Remap12.inputs[0].default_value = (0.000) #BIOS SuitSecondary Wear Remap: [_,_,_,X]
    Wear_Remap12.inputs[1].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap12.inputs[2].default_value = (0.000) #BIOS Empty Value just in case
    Wear_Remap12.location = (0,-1620)
    Wear_Remap12.hide = (0)

    link(Wear_Remap01.outputs[0], WearRemapA.inputs[2])
    link(Wear_Remap03.outputs[0], WearRemapA.inputs[1])
    link(Wear_Remap05.outputs[0], WearRemapB.inputs[2])
    link(Wear_Remap07.outputs[0], WearRemapC.inputs[2])
    link(Wear_Remap09.outputs[0], WearRemapD.inputs[2])
    link(Wear_Remap11.outputs[0], WearRemapE.inputs[2])

    link(Wear_Remap02.outputs[0], WearRemapF.inputs[2])
    link(Wear_Remap04.outputs[0], WearRemapF.inputs[1])
    link(Wear_Remap06.outputs[0], WearRemapG.inputs[2])
    link(Wear_Remap08.outputs[0], WearRemapH.inputs[2])
    link(Wear_Remap10.outputs[0], WearRemapI.inputs[2])
    link(Wear_Remap12.outputs[0], WearRemapJ.inputs[2])


#Gloss Vector A+B Remap XYZ Nodes
    Gloss_Remap01 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap01.label = "GlossRemap"
    Gloss_Remap01.inputs[0].default_value = (0.000) #BIOS ArmorPrimary Roughness Remap [X,_,_,_]
    Gloss_Remap01.inputs[1].default_value = (0.000) #BIOS ArmorPrimary Roughness Remap [_,X,_,_] 
    Gloss_Remap01.inputs[2].default_value = (0.000) #BIOS ArmorPrimary Roughness Remap [_,_,X,_]
    Gloss_Remap01.location = (100,-1900)
    Gloss_Remap01.hide = (0)

    Gloss_Remap02 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap02.label = "GlossRemap"
    Gloss_Remap02.inputs[0].default_value = (0.000) #BIOS ArmorPrimary Roughness Remap [_,_,_,X]
    Gloss_Remap02.inputs[1].default_value = (0.000) #BIOS ArmorPrimary Worn Roughness Remap [X,_,_,_]
    Gloss_Remap02.inputs[2].default_value = (0.000) #BIOS ArmorPrimary Worn Roughness Remap [_,X,_,_]
    Gloss_Remap02.location = (100,-2020)
    Gloss_Remap02.hide = (0)

    Gloss_Remap03 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap03.label = "GlossRemap"
    Gloss_Remap03.inputs[0].default_value = (0.000) #BIOS ArmorSecondary Roughness Remap [X,_,_,_]
    Gloss_Remap03.inputs[1].default_value = (0.000) #BIOS ArmorSecondary Roughness Remap [_,X,_,_]
    Gloss_Remap03.inputs[2].default_value = (0.000) #BIOS ArmorSecondary Roughness Remap [_,_,X,_]
    Gloss_Remap03.location = (100,-2140)
    Gloss_Remap03.hide = (0)

    Gloss_Remap04 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap04.label = "GlossRemap"
    Gloss_Remap04.inputs[0].default_value = (0.000) #BIOS ArmorSecondary Roughness Remap [_,_,_,X]
    Gloss_Remap04.inputs[1].default_value = (0.000) #BIOS ArmorSecondary Worn Roughness Remap [X,_,_,_]
    Gloss_Remap04.inputs[2].default_value = (0.000) #BIOS ArmorSecondary Worn Roughness Remap [_,X,_,_]
    Gloss_Remap04.location = (100,-2260)
    Gloss_Remap04.hide = (0)

    Gloss_Remap05 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap05.label = "GlossRemap"
    Gloss_Remap05.inputs[0].default_value = (0.000) #BIOS ClothPrimary Roughness Remap [X,_,_,_]
    Gloss_Remap05.inputs[1].default_value = (0.000) #BIOS ClothPrimary Roughness Remap [_,X,_,_]
    Gloss_Remap05.inputs[2].default_value = (0.000) #BIOS ClothPrimary Roughness Remap [_,_,X,_]
    Gloss_Remap05.location = (100,-2380)
    Gloss_Remap05.hide = (0)

    Gloss_Remap06 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap06.label = "GlossRemap"
    Gloss_Remap06.inputs[0].default_value = (0.000) #BIOS ClothPrimary Roughness Remap [_,_,_,X]
    Gloss_Remap06.inputs[1].default_value = (0.000) #BIOS ClothPrimary Worn Roughness Remap [X,_,_,_]
    Gloss_Remap06.inputs[2].default_value = (0.000) #BIOS ClothPrimary Worn Roughness Remap [_,X,_,_]
    Gloss_Remap06.location = (100,-2500)
    Gloss_Remap06.hide = (0)

    Gloss_Remap07 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap07.label = "GlossRemap"
    Gloss_Remap07.inputs[0].default_value = (0.000) #BIOS ClothSecondary Roughness Remap [X,_,_,_]
    Gloss_Remap07.inputs[1].default_value = (0.000) #BIOS ClothSecondary Roughness Remap [_,X,_,_]
    Gloss_Remap07.inputs[2].default_value = (0.000) #BIOS ClothSecondary Roughness Remap [_,_,X,_]
    Gloss_Remap07.location = (100,-2620)
    Gloss_Remap07.hide = (0)

    Gloss_Remap08 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap08.label = "GlossRemap"
    Gloss_Remap08.inputs[0].default_value = (0.000) #BIOS ClothSecondary Roughness Remap [_,_,_,X]
    Gloss_Remap08.inputs[1].default_value = (0.000) #BIOS ClothSecondary Worn Roughness Remap [X,_,_,_]
    Gloss_Remap08.inputs[2].default_value = (0.000) #BIOS ClothSecondary Worn Roughness Remap [_,X,_,_]
    Gloss_Remap08.location = (100,-2740)
    Gloss_Remap08.hide = (0)

    Gloss_Remap09 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap09.label = "GlossRemap"
    Gloss_Remap09.inputs[0].default_value = (0.000) #BIOS SuitPrimary Roughness Remap [X,_,_,_]
    Gloss_Remap09.inputs[1].default_value = (0.000) #BIOS SuitPrimary Roughness Remap [_,X,_,_]
    Gloss_Remap09.inputs[2].default_value = (0.000) #BIOS SuitPrimary Roughness Remap [_,_,X,_]
    Gloss_Remap09.location = (100,-2860)
    Gloss_Remap09.hide = (0)

    Gloss_Remap10 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap10.label = "GlossRemap"
    Gloss_Remap10.inputs[0].default_value = (0.000) #BIOS SuitPrimary Roughness Remap [_,_,_,X]
    Gloss_Remap10.inputs[1].default_value = (0.000) #BIOS SuitPrimary Worn Roughness Remap [X,_,_,_]
    Gloss_Remap10.inputs[2].default_value = (0.000) #BIOS SuitPrimary Worn Roughness Remap [_,X,_,_]
    Gloss_Remap10.location = (100,-2980)
    Gloss_Remap10.hide = (0)

    Gloss_Remap11 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap11.label = "GlossRemap"
    Gloss_Remap11.inputs[0].default_value = (0.000) #BIOS SuitSecondary Roughness Remap [X,_,_,_]
    Gloss_Remap11.inputs[1].default_value = (0.000) #BIOS SuitSecondary Roughness Remap [_,X,_,_]
    Gloss_Remap11.inputs[2].default_value = (0.000) #BIOS SuitSecondary Roughness Remap [_,_,X,_]
    Gloss_Remap11.location = (100,-3100)
    Gloss_Remap11.hide = (0)

    Gloss_Remap12 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap12.label = "GlossRemap"
    Gloss_Remap12.inputs[0].default_value = (0.000) #BIOS SuitSecondary Roughness Remap [_,_,_,X]
    Gloss_Remap12.inputs[1].default_value = (0.000) #BIOS SuitSecondary Worn Roughness Remap [X,_,_,_]
    Gloss_Remap12.inputs[2].default_value = (0.000) #BIOS SuitSecondary Worn Roughness Remap [_,X,_,_]
    Gloss_Remap12.location = (100,-3220)
    Gloss_Remap12.hide = (0)

    link(Gloss_Remap01.outputs[0], GlossA.inputs[2])
    link(Gloss_Remap03.outputs[0], GlossA.inputs[1])
    link(Gloss_Remap05.outputs[0], GlossB.inputs[2])
    link(Gloss_Remap07.outputs[0], GlossC.inputs[2])
    link(Gloss_Remap09.outputs[0], GlossD.inputs[2])
    link(Gloss_Remap11.outputs[0], GlossE.inputs[2])

    link(Gloss_Remap02.outputs[0], GlossF.inputs[2])
    link(Gloss_Remap04.outputs[0], GlossF.inputs[1])
    link(Gloss_Remap06.outputs[0], GlossG.inputs[2])
    link(Gloss_Remap08.outputs[0], GlossH.inputs[2])
    link(Gloss_Remap10.outputs[0], GlossI.inputs[2])
    link(Gloss_Remap12.outputs[0], GlossJ.inputs[2])


#Gloss Vector C Remap XYZ Nodes
    Gloss_Remap13 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap13.label = "GlossRemap"
    Gloss_Remap13.inputs[0].default_value = (0.000) #BIOS ArmorPrimary Worn Roughness Remap [_,_,X,_]
    Gloss_Remap13.inputs[1].default_value = (0.000) #BIOS ArmorPrimary Worn Roughness Remap [_,_,_,X]
    Gloss_Remap13.inputs[2].default_value = (0.500) #Value Reserved for Gloss Detail Texture
    Gloss_Remap13.location = (200,-3400)
    Gloss_Remap13.hide = (0)

    Gloss_Remap14 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap14.label = "GlossRemap"
    Gloss_Remap14.inputs[0].default_value = (0.000) #BIOS ArmorSecondary Worn Roughness Remap [_,_,X,_]
    Gloss_Remap14.inputs[1].default_value = (0.000) #BIOS ArmorSecondary Worn Roughness Remap [_,_,_,X]
    Gloss_Remap14.inputs[2].default_value = (0.500) #Value Reserved for Gloss Detail Texture
    Gloss_Remap14.location = (200,-3520)
    Gloss_Remap14.hide = (0)

    Gloss_Remap15 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap15.label = "GlossRemap"
    Gloss_Remap15.inputs[0].default_value = (0.000) #BIOS ClothPrimary Worn Roughness Remap [_,_,X,_]
    Gloss_Remap15.inputs[1].default_value = (0.000) #BIOS ClothPrimary Worn Roughness Remap [_,_,_,X]
    Gloss_Remap15.inputs[2].default_value = (0.500) #Value Reserved for Gloss Detail Texture
    Gloss_Remap15.location = (200,-3640)
    Gloss_Remap15.hide = (0)

    Gloss_Remap16 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap16.label = "GlossRemap"
    Gloss_Remap16.inputs[0].default_value = (0.000) #BIOS ClothSecondary Worn Roughness Remap [_,_,X,_]
    Gloss_Remap16.inputs[1].default_value = (0.000) #BIOS ClothSecondary Worn Roughness Remap [_,_,_,X]
    Gloss_Remap16.inputs[2].default_value = (0.500) #Value Reserved for Gloss Detail Texture
    Gloss_Remap16.location = (200,-3760)
    Gloss_Remap16.hide = (0)

    Gloss_Remap17 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap17.label = "GlossRemap"
    Gloss_Remap17.inputs[0].default_value = (0.000) #BIOS SuitPrimary Worn Roughness Remap [_,_,X,_]
    Gloss_Remap17.inputs[1].default_value = (0.000) #BIOS SuitPrimary Worn Roughness Remap [_,_,_,X]
    Gloss_Remap17.inputs[2].default_value = (0.500) #Value Reserved for Gloss Detail Texture
    Gloss_Remap17.location = (200,-3880)
    Gloss_Remap17.hide = (0)

    Gloss_Remap18 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    Gloss_Remap18.label = "GlossRemap"
    Gloss_Remap18.inputs[0].default_value = (0.000) #BIOS SuitSecondary Worn Roughness Remap [_,_,X,_]
    Gloss_Remap18.inputs[1].default_value = (0.000) #BIOS SuitSecondary Worn Roughness Remap [_,_,_,X]
    Gloss_Remap18.inputs[2].default_value = (0.500) #Value Reserved for Gloss Detail Texture
    Gloss_Remap18.location = (200,-4000)
    Gloss_Remap18.hide = (0)

    link(Gloss_Remap13.outputs[0], GlossK.inputs[2])
    link(Gloss_Remap14.outputs[0], GlossK.inputs[1])
    link(Gloss_Remap15.outputs[0], GlossL.inputs[2])
    link(Gloss_Remap16.outputs[0], GlossM.inputs[2])
    link(Gloss_Remap17.outputs[0], GlossN.inputs[2])
    link(Gloss_Remap18.outputs[0], GlossO.inputs[2])

#Detail Normal Map Texture Nodes
    DetailUV = test_group.nodes.new(type= 'ShaderNodeUVMap')
    DetailUV.label = "Detail UVs"
    DetailUV.uv_map = ("uv1")
    DetailUV.location = (-1300,-300)
    DetailUV.hide = (0)

    TiledNormalMapping01 = test_group.nodes.new(type= 'ShaderNodeMapping')
    TiledNormalMapping01.label = "Detail Normal Transform"
    TiledNormalMapping01.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Armor Detail Normal Transform [_,_,X,X] (These values go in the X and Y)
    TiledNormalMapping01.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Armor Detail Normal Transform [X,X,_,_] (These values go in the X and Y)
    TiledNormalMapping01.location = (-1000,-1500)
    TiledNormalMapping01.hide = (0)

    TiledNormalMapping02 = test_group.nodes.new(type= 'ShaderNodeMapping')
    TiledNormalMapping02.label = "Detail Normal Transform"
    TiledNormalMapping02.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Cloth Detail Normal Transform [_,_,X,X] (These values go in the X and Y)
    TiledNormalMapping02.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Cloth Detail Normal Transform [X,X,_,_] (These values go in the X and Y)
    TiledNormalMapping02.location = (-1000,-1860)
    TiledNormalMapping02.hide = (0)

    TiledNormalMapping03 = test_group.nodes.new(type= 'ShaderNodeMapping')
    TiledNormalMapping03.label = "Detail Normal Transform"
    TiledNormalMapping03.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Suit Detail Normal Transform [_,_,X,X] (These values go in the X and Y)
    TiledNormalMapping03.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Suit Detail Normal Transform [X,X,_,_] (These values go in the X and Y)
    TiledNormalMapping03.location = (-1000,-2220)
    TiledNormalMapping03.hide = (0)


    TiledNormalTexture01 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledNormalTexture01.label = "TiledNormal"
    TiledNormalTexture01.location = (-800,-1600)
    TiledNormalTexture01.hide = (1)

    TiledNormalTexture02 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledNormalTexture02.label = "TiledNormal"
    TiledNormalTexture02.location = (-800,-1720)
    TiledNormalTexture02.hide = (1)

    TiledNormalTexture03 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledNormalTexture03.label = "TiledNormal"
    TiledNormalTexture03.location = (-800,-1840)
    TiledNormalTexture03.hide = (1)

    TiledNormalTexture04 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledNormalTexture04.label = "TiledNormal"
    TiledNormalTexture04.location = (-800,-1960)
    TiledNormalTexture04.hide = (1)

    TiledNormalTexture05 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledNormalTexture05.label = "TiledNormal"
    TiledNormalTexture05.location = (-800,-2080)
    TiledNormalTexture05.hide = (1)

    TiledNormalTexture06 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledNormalTexture06.label = "TiledNormal"
    TiledNormalTexture06.location = (-800,-2200)
    TiledNormalTexture06.hide = (1)



    TiledNormalRGBSeparate01 = test_group.nodes.new(type= 'ShaderNodeSeparateRGB')
    TiledNormalRGBSeparate01.label = "TiledNormal"
    TiledNormalRGBSeparate01.location = (-500,-1600)
    TiledNormalRGBSeparate01.hide = (0)

    TiledNormalRGBSeparate02 = test_group.nodes.new(type= 'ShaderNodeSeparateRGB')
    TiledNormalRGBSeparate02.label = "TiledNormal"
    TiledNormalRGBSeparate02.location = (-500,-1720)
    TiledNormalRGBSeparate02.hide = (0)

    TiledNormalRGBSeparate03 = test_group.nodes.new(type= 'ShaderNodeSeparateRGB')
    TiledNormalRGBSeparate03.label = "TiledNormal"
    TiledNormalRGBSeparate03.location = (-500,-1840)
    TiledNormalRGBSeparate03.hide = (0)

    TiledNormalRGBSeparate04 = test_group.nodes.new(type= 'ShaderNodeSeparateRGB')
    TiledNormalRGBSeparate04.label = "TiledNormal"
    TiledNormalRGBSeparate04.location = (-500,-1960)
    TiledNormalRGBSeparate04.hide = (0)

    TiledNormalRGBSeparate05 = test_group.nodes.new(type= 'ShaderNodeSeparateRGB')
    TiledNormalRGBSeparate05.label = "TiledNormal"
    TiledNormalRGBSeparate05.location = (-500,-2080)
    TiledNormalRGBSeparate05.hide = (0)

    TiledNormalRGBSeparate06 = test_group.nodes.new(type= 'ShaderNodeSeparateRGB')
    TiledNormalRGBSeparate06.label = "TiledNormal"
    TiledNormalRGBSeparate06.location = (-500,-2200)
    TiledNormalRGBSeparate06.hide = (0)



    TiledNormalRGBCombine01 = test_group.nodes.new(type= 'ShaderNodeCombineRGB')
    TiledNormalRGBCombine01.label = "TiledNormal"
    TiledNormalRGBCombine01.inputs[2].default_value = (0.000) #Reserved for Detail Map Opacity
    TiledNormalRGBCombine01.location = (-300,-1600)
    TiledNormalRGBCombine01.hide = (0)

    TiledNormalRGBCombine02 = test_group.nodes.new(type= 'ShaderNodeCombineRGB')
    TiledNormalRGBCombine02.label = "TiledNormal"
    TiledNormalRGBCombine02.inputs[2].default_value = (0.000) #Reserved for Detail Map Opacity
    TiledNormalRGBCombine02.location = (-300,-1720)
    TiledNormalRGBCombine02.hide = (0)

    TiledNormalRGBCombine03 = test_group.nodes.new(type= 'ShaderNodeCombineRGB')
    TiledNormalRGBCombine03.label = "TiledNormal"
    TiledNormalRGBCombine03.inputs[2].default_value = (0.000) #Reserved for Detail Map Opacity
    TiledNormalRGBCombine03.location = (-300,-1840)
    TiledNormalRGBCombine03.hide = (0)

    TiledNormalRGBCombine04 = test_group.nodes.new(type= 'ShaderNodeCombineRGB')
    TiledNormalRGBCombine04.label = "TiledNormal"
    TiledNormalRGBCombine04.inputs[2].default_value = (0.000) #Reserved for Detail Map Opacity
    TiledNormalRGBCombine04.location = (-300,-1960)
    TiledNormalRGBCombine04.hide = (0)

    TiledNormalRGBCombine05 = test_group.nodes.new(type= 'ShaderNodeCombineRGB')
    TiledNormalRGBCombine05.label = "TiledNormal"
    TiledNormalRGBCombine05.inputs[2].default_value = (0.000) #Reserved for Detail Map Opacity
    TiledNormalRGBCombine05.location = (-300,-2080)
    TiledNormalRGBCombine05.hide = (0)

    TiledNormalRGBCombine06 = test_group.nodes.new(type= 'ShaderNodeCombineRGB')
    TiledNormalRGBCombine06.label = "TiledNormal"
    TiledNormalRGBCombine06.inputs[2].default_value = (0.000) #Reserved for Detail Map Opacity
    TiledNormalRGBCombine06.location = (-300,-2200)
    TiledNormalRGBCombine06.hide = (0)


    link(DetailUV.outputs[0], TiledNormalMapping01.inputs[0])
    link(DetailUV.outputs[0], TiledNormalMapping02.inputs[0])
    link(DetailUV.outputs[0], TiledNormalMapping03.inputs[0])

    link(TiledNormalMapping01.outputs[0], TiledNormalTexture01.inputs[0])
    link(TiledNormalMapping01.outputs[0], TiledNormalTexture02.inputs[0])
    link(TiledNormalMapping02.outputs[0], TiledNormalTexture03.inputs[0])
    link(TiledNormalMapping02.outputs[0], TiledNormalTexture04.inputs[0])
    link(TiledNormalMapping03.outputs[0], TiledNormalTexture05.inputs[0])
    link(TiledNormalMapping03.outputs[0], TiledNormalTexture06.inputs[0])

    link(TiledNormalTexture01.outputs[0], TiledNormalRGBSeparate01.inputs[0])
    link(TiledNormalTexture02.outputs[0], TiledNormalRGBSeparate02.inputs[0])
    link(TiledNormalTexture03.outputs[0], TiledNormalRGBSeparate03.inputs[0])
    link(TiledNormalTexture04.outputs[0], TiledNormalRGBSeparate04.inputs[0])
    link(TiledNormalTexture05.outputs[0], TiledNormalRGBSeparate05.inputs[0])
    link(TiledNormalTexture06.outputs[0], TiledNormalRGBSeparate06.inputs[0])

    link(TiledNormalRGBSeparate01.outputs[0], TiledNormalRGBCombine01.inputs[0])
    link(TiledNormalRGBSeparate01.outputs[1], TiledNormalRGBCombine01.inputs[1])
    link(TiledNormalRGBSeparate02.outputs[0], TiledNormalRGBCombine02.inputs[0])
    link(TiledNormalRGBSeparate02.outputs[1], TiledNormalRGBCombine02.inputs[1])
    link(TiledNormalRGBSeparate03.outputs[0], TiledNormalRGBCombine03.inputs[0])
    link(TiledNormalRGBSeparate03.outputs[1], TiledNormalRGBCombine03.inputs[1])
    link(TiledNormalRGBSeparate04.outputs[0], TiledNormalRGBCombine04.inputs[0])
    link(TiledNormalRGBSeparate04.outputs[1], TiledNormalRGBCombine04.inputs[1])
    link(TiledNormalRGBSeparate05.outputs[0], TiledNormalRGBCombine05.inputs[0])
    link(TiledNormalRGBSeparate05.outputs[1], TiledNormalRGBCombine05.inputs[1])
    link(TiledNormalRGBSeparate06.outputs[0], TiledNormalRGBCombine06.inputs[0])
    link(TiledNormalRGBSeparate06.outputs[1], TiledNormalRGBCombine06.inputs[1])

    link(TiledNormalRGBCombine01.outputs[0], TiledNormal_A.inputs[2])
    link(TiledNormalRGBCombine02.outputs[0], TiledNormal_A.inputs[1])
    link(TiledNormalRGBCombine03.outputs[0], TiledNormal_B.inputs[2])
    link(TiledNormalRGBCombine04.outputs[0], TiledNormal_C.inputs[2])
    link(TiledNormalRGBCombine05.outputs[0], TiledNormal_D.inputs[2])
    link(TiledNormalRGBCombine06.outputs[0], TiledNormal_E.inputs[2])

#Detail Diffuse Texture Nodes
    TiledDiffuseMapping01 = test_group.nodes.new(type= 'ShaderNodeMapping')
    TiledDiffuseMapping01.label = "Detail Diffuse Transform"
    TiledDiffuseMapping01.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Armor Detail Diffuse Transform [_,_,X,X] (These values go in the X and Y)
    TiledDiffuseMapping01.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Armor Detail Diffuse Transform [X,X,_,_] (These values go in the X and Y)
    TiledDiffuseMapping01.location = (-1000,-300)
    TiledDiffuseMapping01.hide = (0)

    TiledDiffuseMapping02 = test_group.nodes.new(type= 'ShaderNodeMapping')
    TiledDiffuseMapping02.label = "Detail Diffuse Transform"
    TiledDiffuseMapping02.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Cloth Detail Diffuse Transform [_,_,X,X] (These values go in the X and Y)
    TiledDiffuseMapping02.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Cloth Detail Diffuse Transform [X,X,_,_] (These values go in the X and Y)
    TiledDiffuseMapping02.location = (-1000,-660)
    TiledDiffuseMapping02.hide = (0)

    TiledDiffuseMapping03 = test_group.nodes.new(type= 'ShaderNodeMapping')
    TiledDiffuseMapping03.label = "Detail Diffuse Transform"
    TiledDiffuseMapping03.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Suit Detail Diffuse Transform [_,_,X,X] (These values go in the X and Y)
    TiledDiffuseMapping03.inputs[3].default_value = (1.000, 1.000, 1.000) #BIOS Suit Detail Diffuse Transform [X,X,_,_] (These values go in the X and Y)
    TiledDiffuseMapping03.location = (-1000,-1020)
    TiledDiffuseMapping03.hide = (0)


    TiledDiffuseTexture01 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledDiffuseTexture01.label = "TiledDiffuse"
    TiledDiffuseTexture01.location = (-800,-450)
    TiledDiffuseTexture01.hide = (1)

    TiledDiffuseTexture02 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledDiffuseTexture02.label = "TiledDiffuse"
    TiledDiffuseTexture02.location = (-800,-570)
    TiledDiffuseTexture02.hide = (1)

    TiledDiffuseTexture03 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledDiffuseTexture03.label = "TiledDiffuse"
    TiledDiffuseTexture03.location = (-800,-690)
    TiledDiffuseTexture03.hide = (1)

    TiledDiffuseTexture04 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledDiffuseTexture04.label = "TiledDiffuse"
    TiledDiffuseTexture04.location = (-800,-810)
    TiledDiffuseTexture04.hide = (1)

    TiledDiffuseTexture05 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledDiffuseTexture05.label = "TiledDiffuse"
    TiledDiffuseTexture05.location = (-800,-930)
    TiledDiffuseTexture05.hide = (1)

    TiledDiffuseTexture06 = test_group.nodes.new(type= 'ShaderNodeTexImage')
    TiledDiffuseTexture06.label = "TiledDiffuse"
    TiledDiffuseTexture06.location = (-800,-1060)
    TiledDiffuseTexture06.hide = (1)

    link(DetailUV.outputs[0], TiledDiffuseMapping01.inputs[0])
    link(DetailUV.outputs[0], TiledDiffuseMapping02.inputs[0])
    link(DetailUV.outputs[0], TiledDiffuseMapping03.inputs[0])

    link(TiledDiffuseMapping01.outputs[0], TiledDiffuseTexture01.inputs[0])
    link(TiledDiffuseMapping01.outputs[0], TiledDiffuseTexture02.inputs[0])
    link(TiledDiffuseMapping02.outputs[0], TiledDiffuseTexture03.inputs[0])
    link(TiledDiffuseMapping02.outputs[0], TiledDiffuseTexture04.inputs[0])
    link(TiledDiffuseMapping03.outputs[0], TiledDiffuseTexture05.inputs[0])
    link(TiledDiffuseMapping03.outputs[0], TiledDiffuseTexture06.inputs[0])


#Metalness/Specular/Iridescence Parameters
    MSI_01 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_01.label = "Metalness/Specular/Iridescence"
    MSI_01.inputs[0].default_value = (0.000) #BIOS ArmorPrimary_material_params [_,_,_,X]
    MSI_01.inputs[1].default_value = (0.000) #BIOS ArmorPrimary_material_params [_,_,X,_]
    MSI_01.inputs[2].default_value = (0.000) #BIOS Armorprimary_material_advanced_params [X,_,_,_]
    MSI_01.location = (1000,-1500)
    MSI_01.hide = (0)

    MSI_02 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_02.label = "Metalness/Specular/Iridescence"
    MSI_02.inputs[0].default_value = (0.000) #BIOS ArmorSecondary_material_params [_,_,_,X]
    MSI_02.inputs[1].default_value = (0.000) #BIOS ArmorSecondary_material_params [_,_,X,_]
    MSI_02.inputs[2].default_value = (0.000) #BIOS ArmorSecondary_material_advanced_params [X,_,_,_]
    MSI_02.location = (1000,-1620)
    MSI_02.hide = (0)

    MSI_03 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_03.label = "Metalness/Specular/Iridescence"
    MSI_03.inputs[0].default_value = (0.000) #BIOS ClothPrimary_material_params [_,_,_,X]
    MSI_03.inputs[1].default_value = (0.000) #BIOS ClothPrimary_material_params [_,_,X,_]
    MSI_03.inputs[2].default_value = (0.000) #BIOS ClothPrimary_material_advanced_params [X,_,_,_]
    MSI_03.location = (1000,-1740)
    MSI_03.hide = (0)

    MSI_04 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_04.label = "Metalness/Specular/Iridescence"
    MSI_04.inputs[0].default_value = (0.000) #BIOS ClothSecondary_material_params [_,_,_,X]
    MSI_04.inputs[1].default_value = (0.000) #BIOS ClothSecondary_material_params [_,_,X,_]
    MSI_04.inputs[2].default_value = (0.000) #BIOS ClothSecondary_material_advanced_params [X,_,_,_]
    MSI_04.location = (1000,-1860)
    MSI_04.hide = (0)

    MSI_05 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_05.label = "Metalness/Specular/Iridescence"
    MSI_05.inputs[0].default_value = (0.000) #BIOS SuitPrimary_material_params [_,_,_,X]
    MSI_05.inputs[1].default_value = (0.000) #BIOS SuitPrimary_material_params [_,_,X,_]
    MSI_05.inputs[2].default_value = (0.000) #BIOS SuitPrimary_material_advanced_params [X,_,_,_]
    MSI_05.location = (1000,-1980)
    MSI_05.hide = (0)

    MSI_06 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_06.label = "Metalness/Specular/Iridescence"
    MSI_06.inputs[0].default_value = (0.000) #BIOS SuitSecondary_material_params [_,_,_,X]
    MSI_06.inputs[1].default_value = (0.000) #BIOS SuitSecondary_material_params [_,_,X,_]
    MSI_06.inputs[2].default_value = (0.000) #BIOS SuitSecondary_material_advanced_params [X,_,_,_]
    MSI_06.location = (1000,-2100)
    MSI_06.hide = (0)

    link(MSI_01.outputs[0], MSI_A.inputs[2])
    link(MSI_02.outputs[0], MSI_A.inputs[1])
    link(MSI_03.outputs[0], MSI_B.inputs[2])
    link(MSI_04.outputs[0], MSI_C.inputs[2])
    link(MSI_05.outputs[0], MSI_D.inputs[2])
    link(MSI_06.outputs[0], MSI_E.inputs[2])

#Worn Metalness/Specular/Iridescence Parameters
    MSI_07 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_07.label = "Worn_Metalness/Specular/Iridescence"
    MSI_07.inputs[0].default_value = (0.000) #BIOS ArmorPrimary_worn_material_params [_,_,_,X]
    MSI_07.inputs[1].default_value = (0.000) #BIOS ArmorPrimary_worn_material_params [_,_,X,_]
    MSI_07.inputs[2].default_value = (0.000) #Blank just in case
    MSI_07.location = (1100,-2300)
    MSI_07.hide = (0)

    MSI_08 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_08.label = "Worn_Metalness/Specular/Iridescence"
    MSI_08.inputs[0].default_value = (0.000) #BIOS ArmorSecondary_worn_material_params [_,_,_,X]
    MSI_08.inputs[1].default_value = (0.000) #BIOS ArmorSecondary_worn_material_params [_,_,X,_]
    MSI_08.inputs[2].default_value = (0.000) #Blank just in case
    MSI_08.location = (1100,-2420)
    MSI_08.hide = (0)

    MSI_09 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_09.label = "Worn_Metalness/Specular/Iridescence"
    MSI_09.inputs[0].default_value = (0.000) #BIOS ClothPrimary_worn_material_params [_,_,_,X]
    MSI_09.inputs[1].default_value = (0.000) #BIOS ClothPrimary_worn_material_params [_,_,X,_]
    MSI_09.inputs[2].default_value = (0.000) #Blank just in case
    MSI_09.location = (1100,-2540)
    MSI_09.hide = (0)

    MSI_10 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_10.label = "Worn_Metalness/Specular/Iridescence"
    MSI_10.inputs[0].default_value = (0.000) #BIOS ClothSecondary_worn_material_params [_,_,_,X]
    MSI_10.inputs[1].default_value = (0.000) #BIOS ClothSecondary_worn_material_params [_,_,X,_]
    MSI_10.inputs[2].default_value = (0.000) #Blank just in case
    MSI_10.location = (1100,-2660)
    MSI_10.hide = (0)

    MSI_11 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_11.label = "Worn_Metalness/Specular/Iridescence"
    MSI_11.inputs[0].default_value = (0.000) #BIOS SuitPrimary_worn_material_params [_,_,_,X]
    MSI_11.inputs[1].default_value = (0.000) #BIOS SuitPrimary_worn_material_params [_,_,X,_]
    MSI_11.inputs[2].default_value = (0.000) #Blank just in case
    MSI_11.location = (1100,-2780)
    MSI_11.hide = (0)

    MSI_12 = test_group.nodes.new(type= 'ShaderNodeCombineXYZ')
    MSI_12.label = "Worn_Metalness/Specular/Iridescence"
    MSI_12.inputs[0].default_value = (0.000) #BIOS SuitSecondary_worn_material_params [_,_,_,X]
    MSI_12.inputs[1].default_value = (0.000) #BIOS SuitSecondary_worn_material_params [_,_,X,_]
    MSI_12.inputs[2].default_value = (0.000) #Blank just in case
    MSI_12.location = (1100,-2900)
    MSI_12.hide = (0)

    link(MSI_07.outputs[0], MSI_Worn_A.inputs[2])
    link(MSI_08.outputs[0], MSI_Worn_A.inputs[1])
    link(MSI_09.outputs[0], MSI_Worn_B.inputs[2])
    link(MSI_10.outputs[0], MSI_Worn_C.inputs[2])
    link(MSI_11.outputs[0], MSI_Worn_D.inputs[2])
    link(MSI_12.outputs[0], MSI_Worn_E.inputs[2])






    return test_group
    
    


            
class NODE_OT_TEST(bpy.types.Operator):
    bl_label = "Add D2 Weapons/Armor Shader"
    bl_idname = "node.test_operator"
    
    def execute(self, context):



        custom_node_name = "Input_API_Name" #BIOS Change Nodegroup name dependent on name of shader ripped from API
        my_group = create_test_group(self, context, custom_node_name)
        test_node = context.view_layer.objects.active.active_material.node_tree.nodes.new('ShaderNodeGroup')
        test_node.node_tree = bpy.data.node_groups[my_group.name]
        test_node.use_custom_color = True
        test_node.color = (0.101, 0.170, 0.297)
        
        return {'FINISHED'}
            
    
   
    
    
def register():
    bpy.utils.register_class(NODE_PT_MAINPANEL)
    bpy.utils.register_class(NODE_OT_TEST)
    
    
    


def unregister():
    bpy.utils.unregister_class(NODE_PT_MAINPANEL)
    bpy.utils.unregister_class(NODE_OT_TEST)


if __name__ == "__main__":
    register()