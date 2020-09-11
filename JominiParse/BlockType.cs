namespace JominiParse
{
    public enum BlockType
    {
        none,
        inheritparent,
        inheritgrandparent,
        effect_scope_change,
        condition_scope_change,
        effect_block,
        condition_block,
        function_block,
        list_block,
        schema_block,
    }
}